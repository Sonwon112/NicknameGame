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
    /// �����ϱ� ���� ��� ������ ���� �õ�
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
    /// ���� ���� ��� ȣ��Ǵ� �޼ҵ�
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
    /// Default ��ư �׷��� ǥ��
    /// </summary>
    public void ShowDefaultGroup()
    {
        LoadingWindow.SetActive(false);
        SettingWindow.SetActive(false);
        ExitWindow.SetActive(false);
        DefaultBtnGroup.SetActive(true);
    }

    /// <summary>
    /// ���� â ǥ��
    /// </summary>
    public void ShowSettingWindow()
    {
        SettingWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    /// <summary>
    /// ���� â �����
    /// </summary>
    public void HideSettingWindow()
    {
        ShowDefaultGroup();
    }

    /// <summary>
    /// ���� â ǥ��
    /// </summary>
    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    /// <summary>
    /// ���� â �����
    /// </summary>
    public void HideExitWindow()
    {
        ShowDefaultGroup();
    }

    /// <summary>
    /// ������ ��ư ���� ��� ���ø����̼� ����
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// �������� ������ �޽��������� ó���� ���� �޼ҵ�
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

