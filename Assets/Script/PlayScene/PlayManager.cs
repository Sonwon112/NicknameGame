using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    public GameObject Bingu;
    public GameObject StartPosition;
    public CinemachineVirtualCamera defaultCam;
    public CinemachineVirtualCamera cameraCam;
    public CinemachineDollyCart cart;

    public GameObject btnStart;
    public GameObject txtCount;
    
    private List<string> testList = new List<string>();
    private List<GameObject> participantList = new List<GameObject> ();
    // Start is called before the first frame update
    void Start()
    {
        //test
        testList.Add("test1");
        testList.Add("test2");
        testList.Add("test3");
        testList.Add("test4");

        Vector3 startPos = StartPosition.transform.position;
        Vector3 startPosSize = StartPosition.GetComponent<StartPosition>().Size;

        float startX = startPos.x - ( startPosSize.x / 2 );
        float startZ = startPos.z - ( startPosSize.z / 2 );
        float endX = startPos.x + (startPosSize.x / 2);
        float endZ = startPos.z + (startPosSize.z / 2);

        foreach (string test in testList) {
            int posX = Random.Range((int)startX, (int)endX);
            int posZ = Random.Range((int)startZ, (int)endZ);
            Vector3 pos = new Vector3(posX, 1, posZ);

            GameObject tmpObj = Instantiate(Bingu, pos, Quaternion.identity);
            participantList.Add(tmpObj);
            CharacterMovement tmp = tmpObj.GetComponent<CharacterMovement>();
            tmp.setNickname(test);
        }
    }

    public void StartGame()
    {
        txtCount.SetActive(true);
        txtCount.GetComponent<CountDown>().CountStart();
        btnStart.SetActive(false);
        defaultCam.Priority = 10;
        cameraCam.Priority = 11;
        cart.m_Speed = 3;
    }
    public void raceReady()
    {
        foreach (GameObject tmp in participantList)
        {
            CharacterMovement movement = tmp.GetComponent<CharacterMovement>();
            movement.Ready();
        }
    }

    public void raceStart()
    {
        foreach (GameObject tmp in participantList)
        {
            CharacterMovement movement = tmp.GetComponent<CharacterMovement>();
            movement.StartRace();
        }
    }
}
