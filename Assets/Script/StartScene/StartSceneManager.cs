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

    private SettingData data = SettingData.instance;
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
            data.setSetting(JsonUtility.FromJson<SettingData>(settingText.ToString()));
            inputChannelId.text = data.channelId;
        }
        else
        {
            data = new SettingData("0niyaNicknameGame", "", 80, 80, 80);
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
    /// 저장하기 버튼 선택 경우 Json 파일 저장
    /// </summary>
    public void SaveSetting()
    {
        data.channelId = inputChannelId.text;
        string dataToJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, dataToJson);
        HideSettingWindow();
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

public class SettingData
{
    public static SettingData instance = new SettingData();
    public string token;
    public string channelId;
    public int masterVolume;
    public int bgmVolume;
    public int sfxVolume;

    public SettingData(){}
    public SettingData(string token, string channelId, int masterVolume, int bgmVolume, int sfxVolume)
    {
        this.token = token;
        this.channelId = channelId;
        this.masterVolume = masterVolume;
        this.sfxVolume = sfxVolume;
        this.bgmVolume = bgmVolume;
    }

    public void setSetting(SettingData data)
    {
        token = data.token;
        channelId = data.channelId;
        masterVolume = data.masterVolume;
        sfxVolume = data.sfxVolume;
        bgmVolume = data.bgmVolume;
    }
}