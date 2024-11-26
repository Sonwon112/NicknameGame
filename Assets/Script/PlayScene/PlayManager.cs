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
            // 캠 자율 조작
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CameraReset();
                movingCam.transform.position = virtualCamList[0].transform.position;
                movingCam.Priority = 11;
                movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(true);
            }
            // 캠 그룹표시
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                setCam(0);
            }
            
            
        }
    }

    /// <summary>
    /// 현재 송출할 Cinemachine VirtualCamera를 설정
    /// </summary>
    /// <param name="index">VirtualCamera List 인덱스</param>
    public void setCam(int index)
    {
        CameraReset();
        movingCam.gameObject.GetComponent<MovingCam>().ToggleMovement(false);
        virtualCamList[index].Priority = 11;
    }

    /// <summary>
    /// 게임 시작 메소드
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
    /// 시작 버튼 클릭 시 레이스를 준비 상태로 전환하는 메소드
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
    /// 준비 상태 후 실제로 경주를 시작하게하는 메소드
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
    /// GameManager에서 Manager로 메시지를 전송할 때 사용
    /// </summary>
    /// <param name="msg">GameManager가 전송할 메시지</param>
    public void gettingMessage(string msg)
    {

    }

    /// <summary>
    /// 종료지점 도착시 랭킹에 닉네임을 추가하는 메소드
    /// </summary>
    /// <param name="participant">추가할 참여자 객체</param>
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
    /// 화면에 송출할 랭킹 지정
    /// </summary>
    /// <param name="rankingList">화면에 송출할 top 10 닉네임 리스트</param>
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
    /// 경기 종료 후 리스트 맵으로 이동하는 메소드
    /// </summary>
    public void outMap()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 실시간 랭킹 선택시 해당 플레이어의 VirtualCam으로 이동하기 위해서 Cam을 찾고 Priority를 지정하는 메소드
    /// </summary>
    /// <param name="nickname"> 찾을 플레이어의 이름 </param>
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
    /// 카메라 전환 과정에서 특정하기 어려울 경우 카메라 전부의 priority를 10으로 전환하는 메소드
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
    ///  EventZone에 닿은 경우 카드 및 대상을 뽑기 위해 호출하는 메소드(게임도 일시정지됨)
    /// </summary>
    public void DrawEvent()
    {
        Time.timeScale = 0;
        EventCard.OpenCard(participantList);
    }

    /// <summary>
    /// 탈락 이벤트 발생시 탈락자 리스트에 전달받은 참여자를 추가
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
