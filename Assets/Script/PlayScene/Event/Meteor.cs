using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private GameObject targetPlayer;
    public float speed = 10f;

    public void setTargetPlayer(GameObject targetPlayer)
    {
        this.targetPlayer = targetPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetPlayer.transform.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
