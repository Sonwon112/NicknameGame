using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoudolphPushEvent : Event
{
    public GameObject Roudolph;
    public override void playEvent(CharacterMovement target)
    {
        GameObject roudophTmp = Instantiate(Roudolph,target.transform);
        target.setIsTarget(true);
        roudophTmp.transform.localPosition = new Vector3(-10, 2, 0);
        roudophTmp.transform.eulerAngles = new Vector3(0, 90, 0);
    }
}
