using System;
using UnityEngine;
using System.IO;


public class DataManager : SubClass<GameManager>
{
    // DataManager 안에 선언해야 접근하기 용이함
    public LoginData login; // 로그인 정보
    public CharData[] characters; // 캐릭터 생성 정보
    public SettingsData setting; // 환경설정 정보
    public TextAsset ItemDB; // 아이템 DB

    public int selectedSlotNum;

    string path;
    public string fileName;

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        //임시
        selectedSlotNum = 0;
        characters = new CharData[4];

        // 유니티 기본 설정 경로. PC나 모바일 등등 어디든 프로젝트 이름으로 된 폴더 생김
        path = Application.persistentDataPath + "/";

        login = new LoginData();
        LoadAllSavedData();
    }

    // 저장할 파일 이름과 저장할 클래스를 입력 받아 JSON 형식의 문자열로 바꾼 후, 로컬에 저장
    public void SaveData(string fileName, Data info)
    {
        string data = JsonUtility.ToJson(info);
        File.WriteAllText(path + fileName, data);
    }

    public void SaveData(string fileName, Data[] info)
    {
        string data = JsonUtility.ToJson(info);
        File.WriteAllText(path + fileName, data); // 이건 로컬에 저장. 추후 서버로
    }

    public string LoadData(string fileName)
    {
        string data = File.ReadAllText(path + fileName);      
        return data;
    }

    public bool CheckData(string fileName)
    {
        if (File.Exists(path + fileName))
        {
            return true;
        }
        return false;
    }

    void LoadAllSavedData()
    {
        if (GameManager.Data.CheckData("LoginData")) // 로그인 기록이 있으면 만들어둔 슬롯 갯수가 있을것이므로 불러옴
        {
            login = JsonUtility.FromJson<LoginData>(GameManager.Data.LoadData("LoginData"));
        }
    }

}