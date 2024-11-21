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
    /// ������ ����Ʈ getter
    /// </summary>
    /// <returns>List<String> participantList</returns>
    public List<String> getParticipantList()
    {   
        return participantList;
    }

    /// <summary>
    /// ���� Scene�� Manager ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sceneManager"> ���� Scene�� Manager ������Ʈ</param>
    public void setSceneManager(Manager sceneManager)
    {
        this.sceneManager = sceneManager;
    }

    /// <summary>
    /// ������ �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="channelId">ä��â �����͸� ������ ���� ���� ������ �������� ��Ʈ������ ä�� Id</param>
    /// <exception cref="Exception">���� ���н� Exception �߻��� �� ����</exception>
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
    /// WebSocket ������ �޽����� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="type">�޽����� Ÿ��</param>
    /// <param name="message">������ �޽���</param>
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
    /// �������� �޽����� ������ �� �����ϴ� Handler
    /// </summary>
    /// <param name="sender">�޽����� ���� ������ ����</param>
    /// <param name="e">������ �޽����� ����</param>
    public void onMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        DTO dto = JsonUtility.FromJson<DTO>(e.Data);

        if (dto == null)
        {
            Debug.Log("�������� �����Դϴ�.");
            return;
        }
        if (!dto.token.Equals("0niyaNicknameGame"))
        {
            Debug.Log("�߸��� ��ū�Դϴ�.");
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
    /// �������� ������ ������ ��� ����Ǵ� �ڵ鷯
    /// </summary>
    /// <param name="sender">������ ���� ����</param>
    /// <param name="e">������ ������ ���� ����</param>
    public void onClose(object sender, CloseEventArgs e) {
        Debug.LogWarning(e.Reason);
        sceneManager.gettingMessage("fail");
    }

    /// <summary>
    /// ������ ���� ������ ���� �޼ҵ�
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
    /// ������ ����Ʈ�� �����ϴ� �޼ҵ�
    /// </summary>
    public void ResetParticipantList()
    {
        participantList.Clear();
    }

    /// <summary>
    /// ���ø����̼� ���Ἥ �������� ������ ���� ��
    /// </summary>
    private void OnApplicationQuit()
    {
        DisconnectServer();
    }

}

/// <summary>
/// �������� ��ſ��� ���Ǵ� DTO
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