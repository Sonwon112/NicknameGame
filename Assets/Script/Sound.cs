using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip btnClip;
    private AudioClip CountDownClip;
    private bool currMuteState = true;
    private string bgm_vol = "Volume_BGM";

    [SerializeField] private AudioMixer m_AudioMixer;
    private float prevVolume = 0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        btnClip = (AudioClip)Resources.Load("UI/Audio/tab_button");
        CountDownClip = (AudioClip)Resources.Load("Audio/bgm/CountDown");
        m_AudioMixer.GetFloat(bgm_vol,out prevVolume);
        if (prevVolume == -80f) currMuteState = false;
    }

    public void playBtnSound()
    {   
        if(audioSource != null)
            audioSource.PlayOneShot(btnClip);
    }

    public void playCountDown()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(CountDownClip);
    }

    public void ToggleMute()
    {
        
        if (currMuteState)
        {
            m_AudioMixer.GetFloat(bgm_vol, out prevVolume);
            currMuteState = false;
            m_AudioMixer.SetFloat(bgm_vol, -80f);
        }
        else
        {
            currMuteState = true;
            m_AudioMixer.SetFloat(bgm_vol, prevVolume);
        }
    }

    public bool getMuteState()
    {
        return currMuteState;
    }
}
