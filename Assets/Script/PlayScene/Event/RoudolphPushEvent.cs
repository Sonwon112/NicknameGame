using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoudolphPushEvent : Event
{
    public GameObject Roudolph;
    public Vector3 pos = new Vector3(-10, 2, 0);
    public Vector3 angle = new Vector3(0, 90, 0);

    public override void playEvent(CharacterMovement target)
    {
        GameObject roudophTmp = Instantiate(Roudolph, target.transform);
        target.setIsTarget(true);
        roudophTmp.transform.localPosition = pos;
        roudophTmp.transform.eulerAngles = angle;
    }
}
