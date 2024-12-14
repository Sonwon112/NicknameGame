using System;
using UnityEngine;

public class SettingData
{
    public static SettingData instance = new SettingData();
    //public string token;
    public string channelId;
    public int masterVolume;
    public int bgmVolume;
    public int sfxVolume;

    public SettingData() { }
    public SettingData(string channelId, int masterVolume, int bgmVolume, int sfxVolume)
    {
        //this.token = token;
        this.channelId = channelId;
        this.masterVolume = masterVolume;
        this.sfxVolume = sfxVolume;
        this.bgmVolume = bgmVolume;
    }

    public void setSetting(SettingData data)
    {
        //token = data.token;
        channelId = data.channelId;
        masterVolume = data.masterVolume;
        sfxVolume = data.sfxVolume;
        bgmVolume = data.bgmVolume;
    }
}