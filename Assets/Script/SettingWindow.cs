using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using static UnityEngine.Rendering.DebugUI;

public class SettingWindow : MonoBehaviour
{
    private const string masterName = "Volume_Master";
    private const string bgmName = "Volume_BGM";
    private const string sfxName = "Volume_SFX";

    SettingData settingData = SettingData.instance;
    [Header("UI")]
    public bool usingChannelInput = false;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public TMP_Text masterVolume;
    public TMP_Text bgmVolume;
    public TMP_Text sfxVolume;

    public TMP_InputField inputChannelId;

    [Header("Audio")]
    public AudioMixer m_AuidoMixer;


    // ---------------- Volume Value
    private float prevMaster;
    private float master;
    private float prevBGM;
    private float bgm;
    private float prevSFX;
    private float sfx;

    // --------------- Setting File
    private const string fileName = "settingData.json";
    private string path;
    private string settingText;



    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            settingText = File.ReadAllText(path);
            if (settingText == null) Debug.LogWarning("리소스 로드 실패");
            settingData.setSetting(JsonUtility.FromJson<SettingData>(settingText.ToString()));
            if(usingChannelInput)
                inputChannelId.text = settingData.channelId;
        }
        else
        {
            settingData = new SettingData("", 80, 80, 80);
            string dataToJson = JsonUtility.ToJson(settingData);
            File.WriteAllText(path, dataToJson);
        }


        m_AuidoMixer.SetFloat(masterName, settingData.masterVolume);
        m_AuidoMixer.SetFloat(bgmName, settingData.bgmVolume);
        m_AuidoMixer.SetFloat(sfxName, settingData.sfxVolume);

        SettingValue();
    }

    private void OnEnable()
    {
        SettingValue();
    }

    void updateVolumeText(TMP_Text txt, Slider slider)
    {
        txt.text = Mathf.Round(slider.value * 100) + "%";
    }

    public void ChangeMasterVoluem()
    {
        updateVolumeText(masterVolume, masterSlider);
        master = Remap(0f, 1f, -80f, 20f, masterSlider.value);
        m_AuidoMixer.SetFloat(masterName, master);
    }

    public void ChangeBGMVolume()
    {
        updateVolumeText(bgmVolume, bgmSlider);
        bgm = Remap(0f, 1f, -80f, 20f, bgmSlider.value);
        m_AuidoMixer.SetFloat(bgmName, bgm);
    }

    public void ChangeSFXVolume()
    {
        updateVolumeText(sfxVolume, sfxSlider);
        sfx = Remap(0f, 1f, -80f, 20f, sfxSlider.value);
        m_AuidoMixer.SetFloat(sfxName, sfx);
    }

    /// <summary>
    /// 저장하기 버튼 선택 경우 Json 파일 저장
    /// </summary>
    public void SaveSetting()
    {   
        if(usingChannelInput)
            settingData.channelId = inputChannelId.text;
        settingData.masterVolume = Mathf.RoundToInt(master);
        settingData.bgmVolume = Mathf.RoundToInt(bgm);
        settingData.sfxVolume = Mathf.RoundToInt(sfx);
        string dataToJson = JsonUtility.ToJson(settingData);
        File.WriteAllText(path, dataToJson);
        //HideSettingWindow();
    }

    public void SavePrevSetting()
    {
        m_AuidoMixer.GetFloat(masterName, out prevMaster);
        m_AuidoMixer.GetFloat(bgmName, out prevBGM);
        m_AuidoMixer.GetFloat(sfxName, out prevSFX);
    }

    public void ResetSetting()
    {
        masterSlider.value = Remap(-80f, 20f, 0f, 1f, prevMaster);
        bgmSlider.value = Remap(-80f, 20f, 0f, 1f, prevBGM);
        sfxSlider.value = Remap(-80f, 20f, 0f, 1f, prevSFX);

        m_AuidoMixer.SetFloat(masterName, prevMaster);
        m_AuidoMixer.SetFloat(bgmName, prevBGM);
        m_AuidoMixer.SetFloat (sfxName, prevSFX);
    }

    float Remap(float fromStart, float fromEnd, float toStart, float toEnd, float currVal)
    {
        float fromRange = fromEnd - fromStart;
        float toRange = toEnd - toStart;

        float normalizedValue = (currVal - fromStart) / fromRange;

        return toStart + (normalizedValue * toRange);
    }

    void SettingValue()
    {
        masterSlider.value = Remap(-80f, 20f, 0f, 1f, settingData.masterVolume);
        bgmSlider.value = Remap(-80f, 20f, 0f, 1f, settingData.bgmVolume);
        sfxSlider.value = Remap(-80f, 20f, 0f, 1f, settingData.sfxVolume);
        updateVolumeText(masterVolume, masterSlider);
        updateVolumeText(bgmVolume, bgmSlider);
        updateVolumeText(sfxVolume, sfxSlider);
    }
}
