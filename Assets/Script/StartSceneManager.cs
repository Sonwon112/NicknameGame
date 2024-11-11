using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{

    public GameObject SettingWindow;
    public GameObject ExitWindow;
    public GameObject DefaultBtnGroup;

    public TMP_InputField inputChannelId;

    private SettingData data;
    private TextAsset settingText;

    // Start is called before the first frame update
    void Start()
    {
        settingText = Resources.Load<TextAsset>("settingData");
        if (settingText == null) Debug.LogWarning("리소스 로드 실패");
        data = JsonUtility.FromJson<SettingData>(settingText.ToString());
        /*Debug.Log(settingText.ToString());
        Debug.Log(data.token);
        Debug.Log(data.channelId);
        Debug.Log(data.volume);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSettingWindow()
    {
        SettingWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    public void HideSettingWindow()
    {
        SettingWindow.SetActive(false);
        DefaultBtnGroup.SetActive(true);
    }

    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
        DefaultBtnGroup.SetActive(false);
    }

    public void HideExitWindow()
    {
        ExitWindow.SetActive(false);
        DefaultBtnGroup.SetActive(true);
    }

    public void SaveSetting()
    {
        data.channelId = inputChannelId.text;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    
}

public class SettingData
{
    public string token;
    public string channelId;
    public int volume;
}