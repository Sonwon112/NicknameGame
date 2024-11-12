using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListContent : MonoBehaviour
{
    public int targetSceneIndex;
    public string Title;
    public Sprite Thumbnail;

    private TMP_Text title;
    private Image thumbnail;

    private void Start()
    {
        title = transform.Find("Title").gameObject.GetComponent<TMP_Text>();
        thumbnail = transform.Find("Thumbnail").gameObject.GetComponent<Image>();
        title.text = Title;
        thumbnail.sprite = Thumbnail;
    }
}
