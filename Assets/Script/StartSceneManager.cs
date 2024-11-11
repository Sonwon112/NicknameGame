using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    public GameObject SettingWindow;
    public GameObject ExitWindow;

    private SettingData data;
    private TextAsset settingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSettingWindow()
    {
        SettingWindow.SetActive(true);
    }

    public void HideSettingWindow()
    {
        SettingWindow.SetActive(false);
    }

    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
    }

    public void HideExitWindow()
    {
        ExitWindow.SetActive(false);
    }

    public void SaveSetting()
    {

    }

    public void ExitGame()
    {

    }

}

public class SettingData
{
    private string channelId { get; set; }
    private int volume { get; set; }
}