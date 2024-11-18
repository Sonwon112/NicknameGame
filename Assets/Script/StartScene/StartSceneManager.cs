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

    public TMP_InputField inputChannelId;

    private SettingData data;
    private string settingText;
    private const string fileName = "settingData.json";
    private string path;
    private Thread connectThread;

    public static bool connectFaild = false;
    public static bool LoadNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            settingText = File.ReadAllText(path);
            if (settingText == null) Debug.LogWarning("리소스 로드 실패");
            data = JsonUtility.FromJson<SettingData>(settingText.ToString());
            inputChannelId.text = data.channelId;
        }
        else
        {
            data = new SettingData("0niyaNicknameGame", "", 80);
            string dataToJson = JsonUtility.ToJson(data);
            File.WriteAllText(path, dataToJson);
        }

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
    public void ConnectFailed()
    {
        DefaultBtnGroup.SetActive(true);
        LoadingWindow.SetActive(false);
        Animator LoadingAnimator = LoadingWindow.GetComponentInChildren<Animator>();
        LoadingAnimator.SetBool("isLoading", false);
        ErrorMessage.SetActive(true);

        connectThread.Abort();
    }

    public void ShowDefaultGroup()
    {
        LoadingWindow.SetActive(false);
        SettingWindow.SetActive(false);
        ExitWindow.SetActive(false);
        DefaultBtnGroup.SetActive(true);
    }

    public void ShowSettingWindow()
    {
        SettingWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    public void HideSettingWindow()
    {
        ShowDefaultGroup();
    }

    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    public void HideExitWindow()
    {
        ShowDefaultGroup();
    }

    public void SaveSetting()
    {
        data.channelId = inputChannelId.text;
        string dataToJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, dataToJson);
        HideSettingWindow();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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


}

public class SettingData
{
    public string token;
    public string channelId;
    public int volume;

    public SettingData(string token, string channelId, int volume)
    {
        this.token = token;
        this.channelId = channelId;
        this.volume = volume;
    }
}