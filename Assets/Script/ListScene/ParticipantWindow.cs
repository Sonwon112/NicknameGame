using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantWindow : MonoBehaviour
{
    
    public GameObject ParticipantContent;
    public GameObject ScrollView;
    public Image imgThumbnail;

    private Sprite Thumbnail;
    private int currPosY = -25;
    private int interval = 40;
    private int index = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
