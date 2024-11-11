using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance { get; set; }
    private void Awake()
    {
        if(gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }else if(gameManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private string ip = "";
    private string port = "8080";

    private string serviceName = "/connect";
    public WebSocket m_Socket = null;


    public void ConnectServer(string channelId)
    {
        try
        {
            m_Socket = new WebSocket("ws://"+ip+":"+port+serviceName);
            m_Socket.OnMessage += onMessage;
            m_Socket.OnClose += onClose;
        }
        catch(Exception e){
            Debug.LogException(e);
        }
        Send("CONNECT", channelId);
    }

    public void Send(string type,string message)
    {
        if (!m_Socket.IsAlive) return;
        try
        {
            SendDTO dto = new SendDTO();
            dto.type = type;
            dto.msg = message;

            string classToJson = JsonUtility.ToJson(dto);
            m_Socket.Send(classToJson);
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void onMessage(object sender, MessageEventArgs e)
    {

    }

    public void onClose(object sender, CloseEventArgs e) { 
        
    }

    public void DisconnectServer()
    {
        try
        {
            if (m_Socket == null) return;
            if (m_Socket.IsAlive) m_Socket.Close();
        }catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnApplicationQuit()
    {
        DisconnectServer();
    }

}

public class SendDTO
{
    private string token = "0niyaNicknameGame";
    public string type { get; set; }
    public string msg { get; set; }
}

public class ReceiveDTO
{
    public string type;
    public string msg;
}
