using System;
using UnityEngine;
using System.IO;


public class DataManager : SubClass<GameManager>
{
    // DataManager �ȿ� �����ؾ� �����ϱ� ������
    public LoginData login; // �α��� ����
    public CharData character; // ĳ���� ���� ����
    public SettingsData setting; // ȯ�漳�� ����

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
        // ����Ƽ �⺻ ���� ���. PC�� ����� ��� ���� ������Ʈ �̸����� �� ���� ����
        path = Application.persistentDataPath + "/";

        login = new LoginData();
        // setting = new SettingsData();
        // character = new CharData();

        LoadAllSavedData();     
    }

    // ������ ���� �̸��� ������ Ŭ������ �Է� �޾� Json ���Ϸ� ����
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
        if (GameManager.Data.CheckData("LoginData")) // �α��� ����� ������ ������ ���� ������ �������̹Ƿ� �ҷ���
        {
            login = JsonUtility.FromJson<LoginData>(GameManager.Data.LoadData("LoginData"));
        }
    }
}