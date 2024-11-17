using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour, Manager
{
    public ParticipantWindow participantWindow;
    private ListContent currListContent;

    private bool canPart = false;
    private GameManager gameManager = GameManager.gameManagerInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenContent(string value)
    {
        GameObject gameObject = GameObject.Find(value);
        if(gameObject == null)
        {
            Debug.Log("해당 이름의 컨텐츠 목록이 존재하지 않습니다");
            return;
        }
        currListContent = gameObject.GetComponent<ListContent>();

        participantWindow.setThumbnail(currListContent.Thumbnail);
        participantWindow.gameObject.SetActive(true);
    }

    public void TogglePart()
    {
        canPart = !canPart;
        switch (canPart)
        {
            // 참여 활성시
            case false:
                gameManager.Send(NetworkingType.PERMIT.ToString(), "permit");
                participantWindow.ToggleParticipant(true);
                break;
            // 참여 비활성시
            case true:
                gameManager.Send(NetworkingType.PERMIT.ToString(), "stop");
                participantWindow.ToggleParticipant(false);
                participantWindow.ActivOpenMap();
                break;
        }
    }

    public void gettingMessage(string msg)
    {
        participantWindow.AppendParticipant(msg);
    }

    public void closeParticipantWindow()
    {
        participantWindow.gameObject.SetActive(false);
    }

}
