using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour, Manager
{   

    [Header("Player")]
    public GameObject Bingu;
    public GameObject StartPosition;
    public GameObject EndPosition;

    [Header("Cinemachine")]
    public List<CinemachineVirtualCamera> virtualCamList = new List<CinemachineVirtualCamera>(); // 0 : defaultCam, 1 : countCam, 2 : movingCam, 3 : endPositionCam

    public CinemachineDollyCart cart;
    public CinemachineTargetGroup targetGroup;

    [Header("UI")]
    public GameObject btnStart;
    public GameObject txtCount;
    public GameObject PlayRanking;
    public GameObject EndRankingObj;

    [Header("Header")]
    public EventCard EventCard;
    
    private List<string> nicknameList = new List<string>();
    private List<GameObject> participantList = new List<GameObject> ();

    private List<GameObject> RankinListObject = new List<GameObject> ();
    private List<string> ResultRankingList = new List<string>();
    private List<string> DropOutRankingList = new List<string>();
    public bool isStart;
    public bool isReady = false;
    private GameManager gameManager = GameManager.gameManagerInstance;

    private CinemachineVirtualCamera movingCam;
    // Start is called before the first frame update
    void Start()
    {
        

        if(gameManager != null)
        {
            nicknameList = gameManager.getParticipantList();
        }
        else
        {
            //test
            nicknameList.Add("test1");
            nicknameList.Add("test2");
            nicknameList.Add("test3");
            nicknameList.Add("test4");
            nicknameList.Add("test5");
            nicknameList.Add("test6");
            nicknameList.Add("test7");
            nicknameList.Add("test8");
            nicknameList.Add("test9");
            nicknameList.Add("test10");
            nicknameList.Add("test11");
        }


        Vector3 startPos = StartPosition.transform.position;
        Vector3 startPosSize = StartPosition.GetComponent<StartPosition>().Size;

        float startX = startPos.x - ( startPosSize.x / 2 );
        float startZ = startPos.z - ( startPosSize.z / 2 );
        float endX = startPos.x + (startPosSize.x / 2);
        float endZ = startPos.z + (startPosSize.z / 2);

        List<CinemachineTargetGroup.Target> currTargets = targetGroup.m_Targets.ToList();
        foreach (string test in nicknameList)
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

        if (gameManager != null)
        {
            gameManager.setSceneManager(this);
            gameManager.Send(NetworkingType.RESET.ToString(), "open Map, so require reset list");
        }
    
        for(int i = 1; i <= 10; i++)
        {
            RankinListObject.Add(PlayRanking.transform.Find("PlayRankingContent" + i).gameObject);
        }
    }

    private void Update()
    {
        if (isStart)
        {
            // ķ ���� ����
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CameraReset();
                movingCam.transform.position = virtualCamList[0].transform.position;
                movingCam.Priority = 11;
                movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(true);
            }
            // ķ �׷�ǥ��
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                setCam(0);
            }
            
            
        }
    }

    /// <summary>
    /// ���� ������ Cinemachine VirtualCamera�� ����
    /// </summary>
    /// <param name="index">VirtualCamera List �ε���</param>
    public void setCam(int index)
    {
        CameraReset();
        movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(false);
        virtualCamList[index].Priority = 11;
    }

    /// <summary>
    /// ���� ���� �޼ҵ�
    /// </summary>
    public void StartGame()
    {
        txtCount.SetActive(true);
        txtCount.GetComponent<CountDown>().CountStart();
        btnStart.SetActive(false);
        virtualCamList[0].Priority = 10;
        virtualCamList[1].Priority = 11;
        cart.m_Speed = 5;
        isReady = true;
    }

    /// <summary>
    /// ���� ��ư Ŭ�� �� ���̽��� �غ� ���·� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    public void raceReady()
    {
        foreach (GameObject tmp in participantList)
        {
            CharacterMovement movement = tmp.GetComponent<CharacterMovement>();
            movement.Ready();
        }
    }

    /// <summary>
    /// �غ� ���� �� ������ ���ָ� �����ϰ��ϴ� �޼ҵ�
    /// </summary>
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
        PlayRanking.SetActive(true);
    }

    /// <summary>
    /// GameManager���� Manager�� �޽����� ������ �� ���
    /// </summary>
    /// <param name="msg">GameManager�� ������ �޽���</param>
    public void gettingMessage(string msg)
    {

    }

    /// <summary>
    /// �������� ������ ��ŷ�� �г����� �߰��ϴ� �޼ҵ�
    /// </summary>
    /// <param name="participant">�߰��� ������ ��ü</param>
    public void AppendRank(CharacterMovement participant)
    {
        if (virtualCamList[3].Priority == 10) setCam(3);
        string rankNickname = participant.NicknameText.text;
        ResultRankingList.Add(rankNickname);
        foreach(GameObject tmp in participantList)
        {
            if(tmp.GetComponent<CharacterMovement>().NicknameText.text == rankNickname)
            {
                participantList.Remove(tmp);
                Destroy(participant.gameObject);
                
                if(participantList.Count <= 0)
                {
                    isStart=false;
                    ResultRankingList.AddRange(DropOutRankingList);
                    EndRankingObj.GetComponent<EndRanking>().setRankList(ResultRankingList);
                    EndRankingObj.SetActive(true) ;
                    PlayRanking.SetActive(false);
                }
                return;
            }
        }
    }

    /// <summary>
    /// ȭ�鿡 ������ ��ŷ ����
    /// </summary>
    /// <param name="rankingList">ȭ�鿡 ������ top 10 �г��� ����Ʈ</param>
    public void setRankingList(List<string> rankingList)
    {
        List<string> tmp = new List<string>();
        tmp.AddRange(ResultRankingList);
        tmp.AddRange(rankingList);
        tmp = tmp.Take(rankingList.Count).ToList();
        for(int i = 0; i < tmp.Count; i++)
        {
            RankinListObject[i].GetComponent<PlayRankingContent>().setNickname(tmp[i]);
        }
        tmp.Clear();
    }
    
    /// <summary>
    /// ��� ���� �� ����Ʈ ������ �̵��ϴ� �޼ҵ�
    /// </summary>
    public void outMap()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// �ǽð� ��ŷ ���ý� �ش� �÷��̾��� VirtualCam���� �̵��ϱ� ���ؼ� Cam�� ã�� Priority�� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="nickname"> ã�� �÷��̾��� �̸� </param>
    public void findPlayerCam(TMP_Text nickname)
    {
        foreach(GameObject tmp in participantList)
        {
            if(tmp.GetComponent<CharacterMovement>().NicknameText.text == nickname.text)
            {
                CameraReset();
                tmp.transform.Find("CharacterCamera").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                break;
            }
        }
    }
    /// <summary>
    /// ī�޶� ��ȯ �������� Ư���ϱ� ����� ��� ī�޶� ������ priority�� 10���� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    public void CameraReset()
    {
        GameObject[] virtualCam = GameObject.FindGameObjectsWithTag("VirtualCam");
        foreach(GameObject tmp in virtualCam)
        {
            tmp.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        }
    }

    /// <summary>
    ///  EventZone�� ���� ��� ī�� �� ����� �̱� ���� ȣ���ϴ� �޼ҵ�(���ӵ� �Ͻ�������)
    /// </summary>
    public void DrawEvent()
    {
        Time.timeScale = 0;
        EventCard.OpenCard(participantList);
    }

    /// <summary>
    /// Ż�� �̺�Ʈ �߻��� Ż���� ����Ʈ�� ���޹��� �����ڸ� �߰�
    /// </summary>
    /// <param name="target"></param>
    public void AppendDropOut(CharacterMovement participant)
    {
        string rankNickname = participant.NicknameText.text;
        
        foreach (GameObject tmp in participantList)
        {
            if (tmp.GetComponent<CharacterMovement>().NicknameText.text == rankNickname)
            {
                DropOutRankingList.Insert(0, rankNickname);
                participantList.Remove(tmp);
                //Destroy(participant.gameObject);

                if (participantList.Count <= 0)
                {
                    isStart = false;
                    ResultRankingList.AddRange(DropOutRankingList);
                    EndRankingObj.GetComponent<EndRanking>().setRankList(ResultRankingList);
                    EndRankingObj.SetActive(true);
                    PlayRanking.SetActive(false);
                }
                return;
            }
        }
    }

}
