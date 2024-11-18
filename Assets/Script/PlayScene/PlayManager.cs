using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour, Manager
{   

    [Header("Player")]
    public GameObject Bingu;
    public GameObject StartPosition;

    [Header("Cinemachine")]
    public List<CinemachineVirtualCamera> virtualCamList = new List<CinemachineVirtualCamera>(); // 0 : defaultCam, 1 : countCam, 2 : movingCam;

    public CinemachineDollyCart cart;
    public CinemachineTargetGroup targetGroup;

    [Header("UI")]
    public GameObject btnStart;
    public GameObject txtCount;
    
    private List<string> testList = new List<string>();
    private List<GameObject> participantList = new List<GameObject> ();
    private List<string> RankingList = new List<string> ();
    private bool isStart;
    private GameManager gameManager = GameManager.gameManagerInstance;

    private CinemachineVirtualCamera movingCam;
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

        List<CinemachineTargetGroup.Target> currTargets = targetGroup.m_Targets.ToList();
        foreach (string test in testList)
        {
            int posX = Random.Range((int)startX, (int)endX);
            int posZ = Random.Range((int)startZ, (int)endZ);
            Vector3 pos = new Vector3(posX, 1, posZ);

            GameObject tmpObj = Instantiate(Bingu, pos, Quaternion.identity);
            participantList.Add(tmpObj);

            CharacterMovement tmp = tmpObj.GetComponent<CharacterMovement>();
            tmp.setNickname(test);

            CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
            {
                target = tmpObj.transform,
                weight = 1f,
                radius = 1f
            };

            currTargets.Add(newTarget);
        }
        
        targetGroup.m_Targets = currTargets.ToArray();
        
        foreach(CinemachineVirtualCamera tmp in virtualCamList)
        {
            tmp.Priority = 10;
        }
        virtualCamList[0].Priority = 11;
        movingCam = virtualCamList[2];

        gameManager.setSceneManager(this);
        gameManager.Send(NetworkingType.RESET.ToString(), "open Map, so require reset list");
    }

    private void Update()
    {
        if (isStart)
        {
            // Ä· ÀÚÀ² Á¶ÀÛ
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                foreach (CinemachineVirtualCamera tmp in virtualCamList)
                {
                    tmp.Priority = 10;
                }
                movingCam.Priority = 11;
                movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(true);
            }
            // Ä· ±×·ìÇ¥½Ã
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (CinemachineVirtualCamera tmp in virtualCamList)
                {
                    tmp.Priority = 10;
                }
                movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(false);
                virtualCamList[0].Priority = 11;
            }
        }

    }


    public void StartGame()
    {
        txtCount.SetActive(true);
        txtCount.GetComponent<CountDown>().CountStart();
        btnStart.SetActive(false);
        virtualCamList[0].Priority = 10;
        virtualCamList[1].Priority = 11;
        cart.m_Speed = 5;
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
        isStart = true;
        foreach (GameObject tmp in participantList)
        {
            CharacterMovement movement = tmp.GetComponent<CharacterMovement>();
            movement.StartRace();
        }
        virtualCamList[0].Priority = 11;
        virtualCamList[1].Priority = 10;

    }

    public void gettingMessage(string msg)
    {

    }

    public void AppendRank(Participant participant)
    {
        RankingList.Append(participant.nickname);
        foreach(GameObject tmp in participantList)
        {
            if(tmp.GetComponent<Participant>().id == participant.id)
            {
                participantList.Remove(tmp);
                Destroy(participant.gameObject);
            }
        }
    }
}
