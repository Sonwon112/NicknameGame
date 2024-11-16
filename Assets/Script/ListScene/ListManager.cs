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
            Debug.Log("�ش� �̸��� ������ ����� �������� �ʽ��ϴ�");
            return;
        }
        currListContent = gameObject.GetComponent<ListContent>();

        participantWindow.setThumbnail(currListContent.Thumbnail);
        participantWindow.gameObject.SetActive(true);
    }

    public void TogglePart()
    {
        switch (canPart)
        {
            case false:

                break;
            case true:

                break;
        }
    }

    public void gettingMessage(string msg)
    {
        participantWindow.AppendParticipant(msg);
    }

}
