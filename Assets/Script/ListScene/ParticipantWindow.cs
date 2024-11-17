using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantWindow : MonoBehaviour
{
    
    public GameObject ParticipantContent;
    public GameObject ScrollView;
    public Image imgThumbnail;
    public TMP_Text txtMatch;
    public Button btnOpenMap;

    private Sprite Thumbnail;
    private int currPosY = -25;
    private int interval = 40;
    private int index = 1;

    public void setThumbnail(Sprite Thumbnail)
    {
        this.Thumbnail = Thumbnail;
        imgThumbnail.sprite = Thumbnail;
    }

    public void AppendParticipant(string nickname)
    {
        GameObject listTmp = Instantiate(ParticipantContent,ScrollView.transform);
        listTmp.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, currPosY, 0);
        listTmp.GetComponent<Participant>().id = index++;
        listTmp.GetComponent<Participant>().nickname = nickname;
        currPosY -= interval;
    }

    public void ToggleParticipant(bool isPart)
    {
        switch(isPart)
        {
            case true:
                txtMatch.text = "참여 금지";
                break;
            case false:
                txtMatch.text = "참여 허용";
                break;
        }
    }

    public void ActivOpenMap()
    {
        btnOpenMap.interactable = true;
    }
}
