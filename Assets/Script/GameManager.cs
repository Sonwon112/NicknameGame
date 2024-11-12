using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private string ip = "127.0.0.1";
    private string port = "8080";

    private string serviceName = "/connect";
    public WebSocket m_Socket;
    private static bool connectSuccess = false;

    private void Update()
    {
        if (connectSuccess)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ConnectServer(string channelId)
    {
        try
        {
            m_Socket = new WebSocket("ws://"+ip+":"+port+serviceName);
            m_Socket.OnMessage += onMessage;
            m_Socket.OnClose += onClose;
            m_Socket.Connect();
        }
        catch(Exception e){
            Debug.LogException(e);
            throw new Exception(e.Message);
        }
        if (!m_Socket.IsAlive) throw new Exception("can't Connection");
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
        Debug.Log(e.Data);
        ReceiveDTO dto = JsonUtility.FromJson<ReceiveDTO>(e.Data);

        if (dto == null)
        {
            Debug.Log("부적절한 접근입니다.");
            return;
        }
        if (!dto.token.Equals("0niyaNicknameGame"))
        {
            Debug.Log("잘못된 토큰입니다.");
            return;
        }
        NetworkingType type = (NetworkingType)Enum.Parse(typeof(NetworkingType), dto.type);

        switch (type)
        {
            case NetworkingType.CONNECT:
                connectSuccess = true;
                break;
            case NetworkingType.PERMIT:

                break;
            case NetworkingType.END:

                break;
        }


    }

    public void onClose(object sender, CloseEventArgs e) {
        Debug.Log(e.Reason);
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
    public string token = "0niyaNicknameGame";
    public string type;
    public string msg;
}

public class ReceiveDTO
{
    public string token;
    public string type;
    public string msg;
}

public enum NetworkingType
{
    CONNECT,
    PERMIT,
    END
}