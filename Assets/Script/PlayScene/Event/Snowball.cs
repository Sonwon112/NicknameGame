using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public CinemachineVirtualCamera snowballCam;
    public GameObject snowBall;
    public float speed = 3f;

    private bool isTumble = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTumble)
        {
            snowBall.transform.Translate(Vector3.forward*speed*Time.deltaTime);
        }
        
    }
    public void TumbleSnowball()
    {
        snowballCam.Priority = 10;
        isTumble = true;
    }
}
