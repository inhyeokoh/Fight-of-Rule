using System;
using UnityEngine;
using System.IO;


public class DataManager : SubClass<GameManager>
{
    public CharOption character;
    string path;

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
    }

    public void CreateChar()
    {
        character = new CharOption();
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