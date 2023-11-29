using System;
using UnityEngine;
using System.IO;


public class DataManager : SubClass<GameManager>
{
    // DataManager 안에 선언해야 접근하기 용이함
    public CharData character; // 캐릭터 생성 정보
    public SettingsData setting; // 환경설정 정보

    string path;

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        // 유니티 기본 설정 경로. PC나 모바일 등등 어디든 프로젝트 이름으로 된 폴더 생김
        path = Application.persistentDataPath + "/";

        character = new CharData();
        setting = new SettingsData();
    }

    public void SaveData(string fileName, Data info)
    {
        string data = JsonUtility.ToJson(info);
        File.WriteAllText(path + fileName, data);
        Debug.Log(path);
    }

    public string LoadData(string fileName)
    {
        string data = File.ReadAllText(path + fileName);
        return data;
    }
}