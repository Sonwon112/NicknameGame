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
    private int index = 1;

    public void setThumbnail(Sprite Thumbnail)
    {
        this.Thumbnail = Thumbnail;
        imgThumbnail.sprite = Thumbnail;
    }

    public void AppendParticipant(string nickname)
    {
        GameObject listTmp = Instantiate(ParticipantContent,ScrollView.transform);
        listTmp.GetComponent<Participant>().id = index++;
        listTmp.GetComponent<Participant>().nickname = nickname;
    }

    public void ClearParticiapnt()
    {
       foreach(Transform child in ScrollView.transform)
        {
            Destroy(child.gameObject);
        }
        setActiveOpenMap(false);
        index = 1;
    }

    public void ToggleParticipant(bool isPart)
    {
        switch(isPart)
        {
            case true:
                txtMatch.text = "참여 종료";
                break;
            case false:
                txtMatch.text = "참여 허용";
                break;
        }
    }

    public void setActiveOpenMap(bool active)
    {
        btnOpenMap.interactable = active;
    }
}
