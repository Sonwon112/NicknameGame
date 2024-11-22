using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private GameObject MainCam;
    // Start is called before the first frame update
    void Start()
    {
        MainCam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = MainCam.transform.position;

        this.transform.LookAt(camPos);
    }
}
