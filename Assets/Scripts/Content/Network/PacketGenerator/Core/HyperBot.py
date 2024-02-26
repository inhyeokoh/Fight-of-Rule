from slack_bolt import App
from slack_bolt.adapter.socket_mode import SocketModeHandler

import multiprocessing
import requests
import asyncio
import time
import natsort
import os

import ssl
import certifi

from slack_sdk.web import WebClient


from enum import IntEnum

#BASE_DIR = ".."
TEST_PREFIX = ""
TEST_DIR = "Proto"

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
        
        #ssl._create_default_https_context = ssl._create_unverified_context
        explicit_cert_ctx = ssl.create_default_context(cafile=certifi.where())
        
        test_client = WebClient(token=self.app_token, ssl=explicit_cert_ctx)
        
        self.app = App(
            token=app_token,
            client=test_client,
            #ssl=ignore_context
            request_verification_enabled=False,
            ssl_check_enabled=False
            )
        
        self.handle_table = {}
        self.handle_table[int(AnsItemType.EVT)] = self.handle_evt
        self.handle_table[int(AnsItemType.MSG)] = self.handle_msg
        
        self.ask_dic = {}
        
        self.init_event_handlers()
        
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
        
        
    def send_init(self):
        send_items = f'./{TEST_PREFIX}{TEST_DIR}/'
        proto_files = natsort.natsorted((os.listdir(send_items)))
        proto_files = [protofile for protofile in proto_files if protofile.endswith('.proto')]
        
        to_upload_files = []
        
        for proto_file in proto_files:
            file_response = self.app.client.files_upload(
                file=f"./{TEST_PREFIX}{TEST_DIR}/{proto_file}",  # 업로드할 파일의 경로
                filename=f"{proto_file}"
            )

            to_upload_files.append(file_response)
            
        response = ""
        for ufile in to_upload_files:
            response = response + "<" + ufile["file"]["permalink"] + "| >"
            
            
        out_p  = self.app.client.chat_postMessage(
            #channel="C05QK7DEZEX",  # 요청한 채널로 응답
            channel="C06M660FUD6", #bottest
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
        @self.app.message()
        def respond_to_keyword(ack, say, message):
            ack()
            #self.queue.put(AnsItem(AnsItemType.MSG, message, say))
            with self.lock:
                self.con_queue.append(AnsItem(AnsItemType.MSG, message, say))

        # 파일 업로드 이벤트 핸들러
        @self.app.event("message")
        def handle_file_share(event, say):
            #self.queue.put(AnsItem(AnsItemType.EVT, event, say))
            
            # 메시지가 파일 공유(subtype이 'file_share') 이벤트인 경우에만 처리
            #if event.get("subtype") == "file_share":
            with self.lock:
                self.con_queue.append(AnsItem(AnsItemType.EVT, event, say))
    
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
            
            

    def parse_keyword(self, message, say):
        try:
            files = message["files"]
            self.parse_files(files, say, message["ts"], message["channel"])
        except:
            return
    
    
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
            self.parse_files(files, say, ts, event.get("channel"))
            
    
    def handle_msg(self, message, say):
        self.parse_keyword(message, say)