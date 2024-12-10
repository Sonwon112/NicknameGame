using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMuteImage : MonoBehaviour
{
    private Image btnImage;
    [SerializeField]private Manager playManager;
    [SerializeField] private Sprite mute;
    [SerializeField] private Sprite noneMute;

    private void Start()
    {
        btnImage = GetComponent<Image>();
        if (playManager == null && GameObject.FindWithTag("Manager") != null)
        {
            playManager = GameObject.FindWithTag("Manager").GetComponent<Manager>();
        }
        ToggleImage();


    }
    public void ToggleImage()
    {
        if(playManager.getMuteState()) {
            btnImage.sprite = noneMute;
        }
        else
        {
            btnImage.sprite = mute;
        }
    }
}
