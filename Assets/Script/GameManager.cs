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
    private static string TOKEN = "0niyaNicknameGame";
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
    private List<string> participantList = new List<string>();
    private Manager sceneManager;

    private void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    private void Update()
    {
        
    }

    
    /// <summary>
    /// 참여자 리스트 getter
    /// </summary>
    /// <returns>List<String> participantList</returns>
    public List<String> getParticipantList()
    {   
        return participantList;
    }

    /// <summary>
    /// 현재 Scene의 Manager 오브젝트를 지정하는 함수
    /// </summary>
    /// <param name="sceneManager"> 현재 Scene의 Manager 오브젝트</param>
    public void setSceneManager(Manager sceneManager)
    {
        this.sceneManager = sceneManager;
    }

    /// <summary>
    /// 서버에 연결하는 메소드
    /// </summary>
    /// <param name="channelId">채팅창 데이터를 얻어오기 위한 현재 게임을 실행중인 스트리머의 채널 Id</param>
    /// <exception cref="Exception">연결 실패시 Exception 발생할 수 있음</exception>
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
        Send(NetworkingType.CONNECT.ToString(), channelId);
    }
    /// <summary>
    /// WebSocket 서버로 메시지를 전송하는 메소드
    /// </summary>
    /// <param name="type">메시지의 타입</param>
    /// <param name="message">전송할 메시지</param>
    public void Send(string type,string message)
    {
        if (!m_Socket.IsAlive) return;
        try
        {
            DTO dto = new DTO();
            dto.token = TOKEN;
            dto.type = type;
            dto.msg = message;

            string classToJson = JsonUtility.ToJson(dto);
            Debug.Log(classToJson);
            m_Socket.Send(classToJson);
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// 서버에서 메시지를 보냈을 때 반응하는 Handler
    /// </summary>
    /// <param name="sender">메시지를 보낸 서버의 정보</param>
    /// <param name="e">수신한 메시지의 정보</param>
    public void onMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        DTO dto = JsonUtility.FromJson<DTO>(e.Data);

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
                participantList.Add(dto.msg);
                break;
            case NetworkingType.END:

                break;
        }
        sceneManager.gettingMessage(dto.msg);


    }

    /// <summary>
    /// 서버와의 연결이 끊겼을 경우 실행되는 핸들러
    /// </summary>
    /// <param name="sender">연결이 끊긴 서버</param>
    /// <param name="e">연결이 끊겼을 때의 정보</param>
    public void onClose(object sender, CloseEventArgs e) {
        Debug.LogWarning(e.Reason);
        sceneManager.gettingMessage("fail");
    }

    /// <summary>
    /// 서버로 부터 연결을 끊는 메소드
    /// </summary>
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

    /// <summary>
    /// 참여자 리스트를 리셋하는 메소드
    /// </summary>
    public void ResetParticipantList()
    {
        participantList.Clear();
    }

    /// <summary>
    /// 애플리케이션 종료서 서버와의 연결을 끊게 함
    /// </summary>
    private void OnApplicationQuit()
    {
        DisconnectServer();
    }

}

/// <summary>
/// 서버와의 통신에서 사용되는 DTO
/// 
/// </summary>
public class DTO
{
    public string token;
    public string type;
    public string msg;
}

public enum NetworkingType
{
    CONNECT,
    PERMIT,
    RESET,
    END
}