using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public GameObject standard;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(standard.transform.position, Vector3.up,speed*Time.smoothDeltaTime);
    }
}
