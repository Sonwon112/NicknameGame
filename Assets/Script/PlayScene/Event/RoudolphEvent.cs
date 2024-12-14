using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoudolphEvent : Event
{
    public GameObject Roudolph;
    public override void playEvent(CharacterMovement target)
    {
        GameObject roudophTmp = Instantiate(Roudolph,target.transform);
        target.setIsTarget(true);
        roudophTmp.transform.localPosition = new Vector3(0, 2, 10);
        roudophTmp.transform.localRotation = new Quaternion(0,180,0,0);
    }
}
