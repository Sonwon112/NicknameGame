using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour, Manager
{

    public GameObject SettingWindow;
    public GameObject ExitWindow;
    public GameObject LoadingWindow;
    public GameObject DefaultBtnGroup;
    public GameObject ErrorMessage;

    public GameManager gameManager;

    public static bool connectFaild = false;
    public static bool LoadNextScene = false;

    private SettingData data = SettingData.instance;
    private Thread connectThread;

    // Start is called before the first frame update
    void Start()
    {
        

        /*Debug.Log(settingText.ToString());
        Debug.Log(data.token);
        Debug.Log(data.channelId);
        Debug.Log(data.volume);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (connectFaild)
        {
            ConnectFailed();
            connectFaild = false;
        }
        if (LoadNextScene)
        {
            SceneManager.LoadScene(1);
            LoadNextScene = false;
        }
    }

    /// <summary>
    /// 시작하기 누를 경우 서버에 연결 시도
    /// </summary>
    public void ConnectServer()
    {
        DefaultBtnGroup.SetActive(false);
        LoadingWindow.SetActive(true);
        Animator LoadingAnimator = LoadingWindow.GetComponentInChildren<Animator>();
        LoadingAnimator.SetBool("isLoading", true);
        ErrorMessage.SetActive(false);

        connectThread = new Thread(() =>
        {
            try
            {
                gameManager.ConnectServer(data.channelId);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                connectFaild = true;
            }
        });
        connectThread.Start();
    }

    /// <summary>
    /// 연결 실패 경우 호출되는 메소드
    /// </summary>
    public void ConnectFailed()
    {
        DefaultBtnGroup.SetActive(true);
        LoadingWindow.SetActive(false);
        Animator LoadingAnimator = LoadingWindow.GetComponentInChildren<Animator>();
        LoadingAnimator.SetBool("isLoading", false);
        ErrorMessage.SetActive(true);

        connectThread.Abort();
    }

    /// <summary>
    /// Default 버튼 그룹을 표시
    /// </summary>
    public void ShowDefaultGroup()
    {
        LoadingWindow.SetActive(false);
        SettingWindow.SetActive(false);
        ExitWindow.SetActive(false);
        DefaultBtnGroup.SetActive(true);
    }

    /// <summary>
    /// 설정 창 표시
    /// </summary>
    public void ShowSettingWindow()
    {
        SettingWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    /// <summary>
    /// 설정 창 숨기기
    /// </summary>
    public void HideSettingWindow()
    {
        ShowDefaultGroup();
    }

    /// <summary>
    /// 종료 창 표시
    /// </summary>
    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    /// <summary>
    /// 종료 창 숨기기
    /// </summary>
    public void HideExitWindow()
    {
        ShowDefaultGroup();
    }

    /// <summary>
    /// 나가기 버튼 선택 경우 애플리케이션 종료
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 서버에서 전송한 메시지에대한 처리를 위한 메소드
    /// </summary>
    /// <param name="msg"></param>
    public void gettingMessage(string msg)
    {
        if (msg.Equals("success"))
        {
            LoadNextScene = true;
        }
        if (msg.Equals("fail"))
        {
            connectFaild = true;
        }
    }

    public bool getMuteState()
    {
        return GetComponent<Sound>().getMuteState();
    }

}

