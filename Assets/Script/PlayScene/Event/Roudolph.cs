using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roudolph : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 direction = Vector3.forward;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed*Time.deltaTime);
    }
}
