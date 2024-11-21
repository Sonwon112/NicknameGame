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
    private const float DEFAULT_MIN_SPEED = 3f;
    private const float DEFAULT_MAX_SPEED = 4f;
    private const float FAST_SPEED = 6f;


    private bool isReady = false;
    private bool isRaceStart = false;
    private bool isSpeedUp = false;

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
            if(currTime - prevTime >= 5)
            {
                speedDown();
            }
        }


        if (isRaceStart)
        {
            currTime = DateTime.Now.TimeOfDay.Seconds;
            if(currTime - prevTime >= 3)
            {
                speed = changeSpeed();
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
        speed = 6f;
        characterAnim.setSpeed(speed);
    }

    /// <summary>
    /// �ӵ��� ���ӵǴ� �̺�Ʈ�� ���ؼ� ȣ��Ǵ� �޼ҵ�� �ִϸ��̼� ��ȯ�� ���̻��
    /// </summary>
    public void speedDown()
    {
        isSpeedUp = false;
        speed = 3f;
        characterAnim.setSpeed(speed);
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
    /// ���� �ð� �������� �ӵ� ��ȭ 3~4
    /// </summary>
    public float changeSpeed()
    {
       return UnityEngine.Random.Range(DEFAULT_MIN_SPEED, DEFAULT_MAX_SPEED);
    }
}
