using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterMovement : MonoBehaviour
{
    public TMP_Text NicknameText;

    private GameObject EndPosition;
    private Vector3 dir;
    
    private float speed= 3f;
    private const float DEFAULT_MIN_SPEED = 7f;
    private const float DEFAULT_MAX_SPEED = 8f;
    private const float FAST_SPEED = 10f;
    private const float SLOW_MIN_SPEED = 3f;
    private const float SLOW_MAX_SPEED = 4f;

    private bool isReady = false;
    private bool isRaceStart = false;
    private bool isSpeedUp = false;
    private bool isTarget = false;
    private bool isShield = false;
    private PlayManager playManager;

    private CharacterAnim characterAnim;

    int prevTime;
    int currTime;

    // Start is called before the first frame update
    void Start()
    {
        EndPosition = GameObject.Find("EndPosition");
        dir = EndPosition.transform.position - transform.position;
        dir.y = 0;
        dir.z = 0;
        dir = dir.normalized;
        //Debug.Log(dir);
    
        characterAnim = GetComponentInChildren<CharacterAnim>();

        speed = changeSpeed();
        playManager = GameObject.Find("PlayManager").GetComponent<PlayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //테스트용
        /*if (Input.GetKeyDown(KeyCode.F1))
        {
            Ready();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            speedUp();
        }*/



        /*if (isReady)
        {
            currTime = DateTime.Now.TimeOfDay.Seconds;
            if (currTime - prevTime >= 3)
            {
                //Debug.Log("Is Start");
                StartRace();
            }

        }*/
        if (isSpeedUp)
        {
            currTime = DateTime.Now.TimeOfDay.Seconds;
            if(currTime - prevTime >= 4)
            {
                speedDown();
            }
        }


        if (isRaceStart)
        {
            characterAnim.setSpeed(speed);
            currTime = DateTime.Now.TimeOfDay.Seconds;
            if(currTime - prevTime >= 3 && !isTarget)
            {
                speed = changeSpeed();
                //characterAnim.setSpeed(speed);
                prevTime = currTime;
            }
            transform.Translate(dir * speed * Time.smoothDeltaTime);
        }
    }

    /// <summary>
    /// 경기 준비 시 호출되는 메소드로 애니메이션 전환에 주로 사용
    /// </summary>
    public void Ready()
    {
        isReady = true;
        prevTime = DateTime.Now.TimeOfDay.Seconds;
        characterAnim.Ready();
    }

    /// <summary>
    /// 경기 시작 시 호출되는 메소드로 애니메이션 전환에 주로 사용
    /// </summary>
    public void StartRace()
    {
        isReady = false;
        isRaceStart = true;
        characterAnim.StartRace();
        prevTime = DateTime.Now.TimeOfDay.Seconds;
    }

    /// <summary>
    /// 속도가 가속되는 이벤트에 대해서 호출되는 메소드로 애니메이션 전환도 같이사용
    /// </summary>
    public void speedUp()
    {
        isSpeedUp = true;
        speed = FAST_SPEED;
        characterAnim.setSpeed(speed);
        prevTime = DateTime.Now.TimeOfDay.Seconds;
    }

    /// <summary>
    /// 속도가 감속되는 이벤트에 대해서 호출되는 메소드로 애니메이션 전환도 같이사용
    /// </summary>
    public void speedDown()
    {
        isSpeedUp = false;
        speed = changeSpeed();
        characterAnim.setSpeed(speed);
    }

    public void speedSlow()
    {
        isSpeedUp = true;
        speed  = UnityEngine.Random.Range(SLOW_MIN_SPEED,SLOW_MAX_SPEED);
        characterAnim.setSpeed(speed);
        prevTime = DateTime.Now.TimeOfDay.Seconds;
    }

    /// <summary>
    /// 최초 참여자 생성시 닉네임을 지정하는 메소드
    /// </summary>
    /// <param name="nickname"> 지정할 닉네임 </param>
    public void setNickname(string nickname)
    {
        NicknameText.text = nickname;
    }

    /// <summary>
    /// 일정 시간 간격으로 속도 변화 DEFAULT_MIN_SPEED~DEAFULT_MAX_SPEED
    /// </summary>
    public float changeSpeed()
    {
        return UnityEngine.Random.Range(DEFAULT_MIN_SPEED, DEFAULT_MAX_SPEED);
    }

    /// <summary>
    /// 충돌 시 발생하는 애니메이션
    /// </summary>
    public void playDownAnim()
    {
        characterAnim.setIsFallFlat(true);
        isTarget = true;
        speed = 0;
    }

    /// <summary>
    /// 미끄러지는 애니메이션 실행
    /// </summary>
    public void playSideFallDown()
    {
        characterAnim.setIsSideFallDown(true);
        speed = 0;
    }

    /// <summary>
    /// 탈락 애니메이션 실행
    /// </summary>
    public void DropOut()
    {
        characterAnim.setDropout(true);
        speed = 0;
        isRaceStart = false;
    }

    /// <summary>
    /// 현재 이벤트의 타겟인지 지정하는 메소드
    /// </summary>
    /// <param name="isTarget">true = 현재 타겟임, false = 타겟이 해제됨</param>
    public void setIsTarget(bool isTarget)
    {
        this.isTarget = isTarget;
        if (!isTarget)
        {
            speed = changeSpeed();
            characterAnim.setSpeed(speed);
            characterAnim.transform.Rotate(Vector3.up, -40);
        }
           
    }

    private GameObject shield;
    public void setIsShield(bool isShield, GameObject shieldTmp)
    {
        this.isShield = isShield;
        shield = shieldTmp;
    }

    /// <summary>
    /// 충돌 감지 Enter
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (isTarget)
        {
            switch (other.tag)
            {
                case "Roudolph":
                    playDownAnim();
                    Destroy(other.gameObject);
                    break;
                case "IcyRoad":
                    playSideFallDown();
                    Destroy(other.gameObject);
                    break;
                case "Snowball":
                    if (isShield)
                    {
                        //speedSlow();
                        isShield = false;
                        Destroy(other.transform.parent.gameObject);
                        Destroy(shield);
                        break;
                    }
                    playDownAnim();
                    break;
            }

        }

        if (other.tag.Equals("GasPlanet"))
        {
            speedSlow();
        }
        if (other.tag.Equals("StonePlanet"))
        {
            playManager.AppendDropOut(this);
            DropOut();
        }

    }

}
