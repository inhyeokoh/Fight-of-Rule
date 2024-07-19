from slack_bolt import App
from slack_bolt.adapter.socket_mode import SocketModeHandler

import multiprocessing
import requests
import asyncio
import time
import natsort
import os
import re
import subprocess

import ssl
import certifi

from enum import IntEnum
from slack_sdk.web import WebClient

#BASE_DIR = ".."
TEST_DIR = "Proto"
CLIENT_DIR = './__build_result__/ProtoBuild'

class AnsItemType(IntEnum):
    EVT = 0,
    MSG = 1,

class AnsItem():
    def __init__(self, type : AnsItemType, *args):
        self.type = type
        self.args = args

#slave version of HyperSlackBot
class HyperSlackBot_Slave():
    def __init__(self, app_token, evt_token):
        self.app_token = app_token
        self.evt_token = evt_token
        
        self.ask_ts = ""
        
        self.manager = multiprocessing.Manager()
        self.lock = self.manager.Lock()
        self.con_queue = []
        
        explicit_cert_ctx = ssl.create_default_context(cafile=certifi.where())
        test_client = WebClient(token=self.app_token, ssl=explicit_cert_ctx)
        
        
        self.app = App(
            token=app_token,
            client=test_client,
            #ssl=ignore_context
            request_verification_enabled=False,
            ssl_check_enabled=False
            )
        
        self.stash_info = -1
        
        self.handle_table = {}
        self.handle_table[int(AnsItemType.EVT)] = self.handle_evt
        self.handle_table[int(AnsItemType.MSG)] = self.handle_msg
        
        self.ask_dic = {}
        
        self.init_event_handlers()
        
        self.bot_user_id = self.app.client.auth_test()["user_id"]
        self.bot_channel = "C06M660FUD6" #bottest
        self.bot_main = 'U06M3SUPU1W' #봇양반
        
        self.parse_files_func = self.parse_files
        self.parse_keyword_func = self.parse_keyword
        
        self.bot = SocketModeHandler(
            self.app, 
            evt_token
            )
         
        
    def start(self):
        loop = asyncio.get_event_loop()
        print("open websock")
        loop.run_in_executor(None, self.bot.start) 
        time.sleep(2)
        #loop.run_in_executor(None, self.send_init) 
        print("send req")
        self.send_init()
        print("waiting res")
        self.dequeue_event()
        
    def start_at_server(self):
        self.parse_files_func = self.parse_files_server
        self.parse_keyword_func = self.parse_keyword_server
        
        loop = asyncio.get_event_loop()
        print("open websock")
        loop.run_in_executor(None, self.bot.start) 
        time.sleep(2)
        #loop.run_in_executor(None, self.send_init) 
        print("send req")
        self.send_stash()
        print("waiting res")
        self.dequeue_event()
        
        
    def send_stash(self):
        self.stash_info = 0
        
        response = f"<@{self.bot_main}> stash"
        
        out_p  = self.app.client.chat_postMessage(
            #channel="C05QK7DEZEX",  # 요청한 채널로 응답
            channel=self.bot_channel, #bottest
            text=response
            )
        
        if out_p["ok"]:
            self.ask_ts = out_p["ts"]
            print(self.ask_ts)
            print("success ask stash")
        else:
            print("failed to apply stash")
        
    def send_init(self):
        send_items = f'./{TEST_DIR}/'
        proto_files = natsort.natsorted((os.listdir(send_items)))
        proto_files = [protofile for protofile in proto_files if protofile.endswith('.proto')]
        
        to_upload_files = []
        
        for proto_file in proto_files:
            file_response = self.app.client.files_upload(
                file=f"./{TEST_DIR}/{proto_file}",  # 업로드할 파일의 경로
                filename=f"{proto_file}"
            )

            to_upload_files.append(file_response)
            
        response = ""
        for ufile in to_upload_files:
            response = response + "<" + ufile["file"]["permalink"] + "| >"
            
            
        out_p  = self.app.client.chat_postMessage(
            #channel="C05QK7DEZEX",  # 요청한 채널로 응답
            channel=self.bot_channel, #bottest
            text=response
            )
    
        if out_p["ok"]:
            for proto_file in proto_files:
                self.ask_dic[proto_file.replace(".proto", ".cs")] = False
                
            self.ask_dic["PacketHandler.cs"] = False
            
            self.ask_ts = out_p["ts"]
            response_text = f"파일이 성공적으로 업로드되었습니다."
        else:
            response_text = "파일 업로드에 실패했습니다."
        
    def init_event_handlers(self):
        # 데코레이터로 이벤트 핸들러 등록
        @self.app.message("")
        def respond_to_keyword(ack, say, message):
            if "user" in message and message["user"] == self.bot_user_id:
                return
            
            if message["channel"] != self.bot_channel:
                return
            
            print("on message")
            ack()
            with self.lock:
                self.con_queue.append(AnsItem(AnsItemType.MSG, message, say))

        # 파일 업로드 이벤트 핸들러
        @self.app.event("message")
        def handle_file_share(event, say):
            return 
        #     print("on event with message")
        #     self.queue.put(AnsItem(AnsItemType.EVT, event, say))
            
            # 메시지가 파일 공유(subtype이 'file_share') 이벤트인 경우에만 처리
            #if event.get("subtype") == "file_share":
    
    def dequeue_event(self):
        while True:
            item : AnsItem = None
            with self.lock:
                if len(self.con_queue) > 0:
                    item = self.con_queue[0]
                    del self.con_queue[0]
                    
            if item is None:
                time.sleep(1)
                continue
                
            if item.type not in self.handle_table.keys():
                continue
            
            self.handle_table[int(item.type)](*item.args)
            
    
    def parse_files(self, files, say, ts, chan):
        #now = str(datetime.datetime.now())
        #now = now.replace(" ", "_")
        #now = now.replace(".", "_")
        #now = now.replace(":", "_")
        #os.makedirs(f"{BASE_DIR}/{now}", exist_ok=True)
        
        if files:
            filenames = []
            for file in files:
                result = self.get_file(file)
                if result.endswith(".cs") == False:
                    continue
                filenames.append(result)
            
            if len(filenames) == 0:
                return
            
            txt = ",\n".join(filenames)
            print_text = f"파일을 다운로드 했습니다. 파일명 : {txt}"
            say(
                text=print_text,
                thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
            )
            print(print_text)
            
            if len(self.ask_dic.keys()) > 0:
                for val in self.ask_dic.values():
                    if val == False:
                        return
                
                print("generate job done!")
                exit(0)
                
    def parse_files_server(self, files, say, ts, chan):
        #now = str(datetime.datetime.now())
        #now = now.replace(" ", "_")
        #now = now.replace(".", "_")
        #now = now.replace(":", "_")
        #os.makedirs(f"{BASE_DIR}/{now}", exist_ok=True)
        
        if self.stash_info <= 0 or self.stash_info != len(files):
            print(f"len files failed : {len(files)}")
            return
        
        print(f"on download {self.stash_info}")
        
        if files:
            filenames = []
            for file in files:
                result = self.get_file_server(file)
                filenames.append(result)
            
            if len(filenames) == 0:
                return
            
            txt = ",\n".join(filenames)
            print_text = f"파일을 다운로드 했습니다. 파일명 : {txt}"
            say(
                text=print_text,
                thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
            )
            print(print_text)
                
            print("generate job done!")
            exit(0)
            
              
    def get_file(self, file):
        file_id = file["id"]
        file_info = self.app.client.files_info(file=file_id)
        file_name = file_info["file"]["name"]
        
        if file_name.endswith(".cs") == False:
            return ""
        
        if file_name not in self.ask_dic.keys():
            return ""
        
        if self.ask_dic[file_name] == True:
            return ""
        
        # 파일 정보 가져오기
        file_info = self.app.client.files_info(file=file_id)
        #file_url = file_info["file"]["url_private"]

        download_url = file_info["file"]["url_private"]
        headers = {"Authorization": f"Bearer {self.app_token}"}
        response = requests.get(download_url, headers=headers)
        
        with open(f"{file_name}", "wb") as f:
            f.write(response.content)
        
        self.ask_dic[file_name] = True
        return file_name
    
    def get_file_server(self, file):
        file_id = file["id"]
        file_info = self.app.client.files_info(file=file_id)
        file_name = file_info["file"]["name"]
        
        # 파일 정보 가져오기
        file_info = self.app.client.files_info(file=file_id)
        #file_url = file_info["file"]["url_private"]

        download_url = file_info["file"]["url_private"]
        headers = {"Authorization": f"Bearer {self.app_token}"}
        response = requests.get(download_url, headers=headers)
        
        with open(f"{file_name}", "wb") as f:
            f.write(response.content)
        
        self.ask_dic[file_name] = True
        return file_name
    
    
    def get_links(self, link_msg):
        links = re.findall(r'<(https://[^|]+)\|', link_msg)
        return links
    
    
    def download_from_link_texts(self, links, say, ts, chan, download_mother_path = ''):
        if len(download_mother_path) > 0:
            os.makedirs(download_mother_path, exist_ok=True)
        
        filenames = []
        
        headers = {"Authorization": f"Bearer {self.app_token}"}
        for link in links:
            # Slack API를 통해 파일 정보 가져오기
            file_id = link.split("/")[-2]

            url = f"https://slack.com/api/files.info"
            headers = {
                "Authorization": f"Bearer {self.app_token}"
            }
            params = {
                "file": file_id
            }
            response = requests.get(url, headers=headers, params=params)
            data = response.json()

            download_url = None
            for i in range(5):
                if response.status_code != 200:
                    print(f"파일 다운로드 실패: {link}")
                    say(
                        text=f"파일 다운로드 실패: {link}, 재시도 {i+1}...",
                        thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
                    )
                    time.sleep(1)
                    continue
                try:
                    # 파일 다운로드 링크 얻기
                    download_url = data["file"]["url_private"]
                    filename = data["file"]["name"]
                except:
                    print(f"파일 다운로드 실패: {link}")
                    say(
                        text=f"파일 다운로드 실패: {link}, 재시도 {i+1}...",
                        thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
                    )
                    time.sleep(3)
                    continue
                
                break
            
            if download_url is None:
                print(f"파일 다운로드 실패: {link}")
                say(
                    text=f"파일 다운로드 실패: {link}",
                    thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
                )
                return
            
            response = requests.get(download_url, headers=headers)
            if response.status_code == 200:
                # 파일 저장
                
                if len(download_mother_path) == 0:
                    with open(filename, 'wb') as f:
                        f.write(response.content)
                else:
                    with open(f'{download_mother_path}/{filename}', 'wb') as f:
                        f.write(response.content)
                    
                filenames.append(filename)
                print(f"다음 파일을 다운로드하였습니다: {filename}")
            else:
                print(f"파일 다운로드 실패: {link}")
                
        if len(filenames) == 0:
                return
            
        txt = ",\n".join(filenames)
        print_text = f"파일을 다운로드 했습니다. 파일명 : {txt}"
        say(
            text=print_text,
            thread_ts=ts  # 현재 메시지의 타임스탬프를 스레드로 지정
        )
        print(print_text)

    
    def handle_evt(self, event, say):
        message = event.get("message", {})
        files = event.get("files")
        
        try:
            ts = message["thread_ts"]
        except: 
            ts = event.get("ts")
            
        if ts != self.ask_ts:
            return
        
        if files is None:
            files = message["files"]
        if files:
            self.parse_files_func(files, say, ts, event.get("channel"))
            
    
    def parse_keyword(self, message, say):
        text : str = message["text"]
        print(f"get message : {text}")
        
        has_link = self.get_links(text)
        for link in has_link:
            print(f"link {link}")
            
        if len(has_link) > 0:
            if message["thread_ts"] != self.ask_ts:
                return
            
            self.stash_info = len(has_link)
            
            self.download_from_link_texts(has_link, say, message["ts"], message["channel"], CLIENT_DIR)
            print("generate job done!")
            
            response = f"<@{self.bot_main}> auto stash"
        
            out_p  = self.app.client.chat_postMessage(
                #channel="C05QK7DEZEX",  # 요청한 채널로 응답
                channel=self.bot_channel, #bottest
                text=response
                )
            
            if out_p["ok"]:
                print("success auto stash")
            else:
                print("failed auto stash")
                
            exit(0)
        
    def parse_keyword_server(self, message, say):
        text : str = message["text"]
        user : str = message["user"]
        print(f"get message : {text}")
        print(f"user : {user}")
        
        has_link = self.get_links(text)
        for link in has_link:
            print(f"link {link}")
            
        if len(has_link) > 0:
            if message["thread_ts"] != self.ask_ts:
                return
            
            self.stash_info = len(has_link)
            
            self.download_from_link_texts(has_link, say, message["ts"], message["channel"])
            print("generate job done!") 
            try:
                result = subprocess.run(["copy_protoc.bat"],
                                                    check=True,  # 프로세스가 실패하면 예외 발생
                                                    stdout=subprocess.PIPE,  # 표준 출력을 파이프로 캡처
                                                    stderr=subprocess.PIPE,  # 표준 에러를 파이프로 캡처
                                                    text=True  # 출력을 문자열로 처리 (Python 3.7+)
                                                )
            except subprocess.CalledProcessError as e:
                # 예외가 발생하면 CalledProcessError 객체가 생성됨
                print("Command failed with return code:", e.returncode)
                print("Standard output:", e.stdout)
                print("Standard error:", e.stderr)
                
                say(
                text=f"파일 복사에 실패했습니다.n\n{e.stdout}\n\n{e.stderr}",
                thread_ts=message["ts"]  # 현재 메시지의 타임스탬프를 스레드로 지정
                )
                return
            else:
                print("complete copy files!")
                
            return
            
        user :str = message["user"]
        if user.startswith("U06LUDEKBDX") and text.endswith("auto stash"):
            if "thread_ts" not in message:
                tmp = message['ts']
                if tmp > self.ask_ts:
                    self.ask_ts = tmp
                    print(self.ask_ts)
            else:
                tmp = message['thread_ts']
                if tmp > self.ask_ts:
                    self.ask_ts = tmp
                    print(self.ask_ts)
            return
        else:
            print(f"anonym msg : {user}: {text}")
        
        
    def handle_msg(self, message, say):
        try:
            self.parse_keyword_func(message, say)
        except Exception as e:
            print("handle msg exception!")
            print(e)
            return