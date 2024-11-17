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
    }

    // Update is called once per frame
    void Update()
    {
        //테스트용
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Ready();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            speedUp();
        }



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
            transform.Translate(dir * speed * Time.smoothDeltaTime);
        }
    }

    public void Ready()
    {
        isReady = true;
        prevTime = DateTime.Now.TimeOfDay.Seconds;
        characterAnim.Ready();
    }

    public void StartRace()
    {
        isReady = false;
        isRaceStart = true;
        characterAnim.StartRace();
    }

    public void speedUp()
    {
        isSpeedUp = true;
        speed = 6f;
        characterAnim.setSpeed(speed);
    }

    public void speedDown()
    {
        isSpeedUp = false;
        speed = 3f;
        characterAnim.setSpeed(speed);
    }

    public void setNickname(string nickname)
    {
        NicknameText.text = nickname;
    }
}
