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
        //�׽�Ʈ��
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
    /// ��� �غ� �� ȣ��Ǵ� �޼ҵ�� �ִϸ��̼� ��ȯ�� �ַ� ���
    /// </summary>
    public void Ready()
    {
        isReady = true;
        prevTime = DateTime.Now.TimeOfDay.Seconds;
        characterAnim.Ready();
    }

    /// <summary>
    /// ��� ���� �� ȣ��Ǵ� �޼ҵ�� �ִϸ��̼� ��ȯ�� �ַ� ���
    /// </summary>
    public void StartRace()
    {
        isReady = false;
        isRaceStart = true;
        characterAnim.StartRace();
        prevTime = DateTime.Now.TimeOfDay.Seconds;
    }

    /// <summary>
    /// �ӵ��� ���ӵǴ� �̺�Ʈ�� ���ؼ� ȣ��Ǵ� �޼ҵ�� �ִϸ��̼� ��ȯ�� ���̻��
    /// </summary>
    public void speedUp()
    {
        isSpeedUp = true;
        speed = FAST_SPEED;
        characterAnim.setSpeed(speed);
        prevTime = DateTime.Now.TimeOfDay.Seconds;
    }

    /// <summary>
    /// �ӵ��� ���ӵǴ� �̺�Ʈ�� ���ؼ� ȣ��Ǵ� �޼ҵ�� �ִϸ��̼� ��ȯ�� ���̻��
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
    /// ���� ������ ������ �г����� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="nickname"> ������ �г��� </param>
    public void setNickname(string nickname)
    {
        NicknameText.text = nickname;
    }

    /// <summary>
    /// ���� �ð� �������� �ӵ� ��ȭ DEFAULT_MIN_SPEED~DEAFULT_MAX_SPEED
    /// </summary>
    public float changeSpeed()
    {
        return UnityEngine.Random.Range(DEFAULT_MIN_SPEED, DEFAULT_MAX_SPEED);
    }

    /// <summary>
    /// �浹 �� �߻��ϴ� �ִϸ��̼�
    /// </summary>
    public void playDownAnim()
    {
        characterAnim.setIsFallFlat(true);
        isTarget = true;
        speed = 0;
    }

    /// <summary>
    /// �̲������� �ִϸ��̼� ����
    /// </summary>
    public void playSideFallDown()
    {
        characterAnim.setIsSideFallDown(true);
        speed = 0;
    }

    /// <summary>
    /// Ż�� �ִϸ��̼� ����
    /// </summary>
    public void DropOut()
    {
        characterAnim.setDropout(true);
        speed = 0;
        isRaceStart = false;
    }

    /// <summary>
    /// ���� �̺�Ʈ�� Ÿ������ �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="isTarget">true = ���� Ÿ����, false = Ÿ���� ������</param>
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
    /// �浹 ���� Enter
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
