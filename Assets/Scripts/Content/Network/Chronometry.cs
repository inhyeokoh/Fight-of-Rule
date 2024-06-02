using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronometry : MonoBehaviour
{
    private static Chronometry _instance;
    public static Chronometry Instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<Chronometry>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 패킷 이름과 해당 패킷의 보낸 시간을 저장
    Dictionary<string, DateTime> packetSendTimes = new Dictionary<string, DateTime>();

    float timeoutSeconds = 5f;

    // 패킷 보낼 때 호출
    public void SendPacket(string packetName)
    {
        packetSendTimes[packetName] = DateTime.Now;

        StartCoroutine(CheckTimeout(packetName));
    }

    IEnumerator CheckTimeout(string packetName)
    {
        yield return new WaitForSeconds(timeoutSeconds);

        if (packetSendTimes.ContainsKey(packetName))
        {
            Debug.Log($"{packetName} 타임아웃");
        }
    }

    // 패킷 응답을 받았을 때 항목 제거
    public void ReceivePacket(string packetName)
    {
        if (packetSendTimes.ContainsKey(packetName))
        {
            packetSendTimes.Remove(packetName);
        }
    }
}