using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip btnClip;
    private AudioClip CountDownClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        btnClip = (AudioClip)Resources.Load("UI/Audio/tab_button");
        CountDownClip = (AudioClip)Resources.Load("Audio/bgm/CountDown");
    }

    public void playBtnSound()
    {
        audioSource.PlayOneShot(btnClip);
    }

    public void playCountDown()
    {
        audioSource.PlayOneShot(CountDownClip);
    }
}
