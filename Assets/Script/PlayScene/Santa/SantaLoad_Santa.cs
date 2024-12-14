using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SantaLoad_Santa : MonoBehaviour
{
    public PlayManager playManager;
    public CinemachineVirtualCamera santaCam;
    public GameObject  SantaObject;
    public float speed = 6f;
    public float term;

    private Animator binguAnimator;
    private float prevTime;
    private float currTime;
    private bool run = false;
    
    // Start is called before the first frame update
    void Start()
    {
        binguAnimator = SantaObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playManager.isReady && !run)
        {
            binguAnimator.SetBool("isStart", true);
            run = true;
            santaCam.m_Priority = 12;
            prevTime = DateTime.Now.TimeOfDay.Seconds;
        }

        if (run)
        {
            if(santaCam.m_Priority == 12)
            {
                currTime = DateTime.Now.TimeOfDay.Seconds;
                if (currTime - prevTime >= term) 
                    santaCam.m_Priority = 10;
            }
            
            transform.Translate(Vector3.right*speed*Time.deltaTime);
        }



    }
}
