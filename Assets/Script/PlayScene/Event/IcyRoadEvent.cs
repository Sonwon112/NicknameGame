using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icyRoadEvent : Event
{
    public GameObject icyRoad;

    public override void playEvent(CharacterMovement target)
    {
        GameObject tmp = Instantiate(icyRoad);
        tmp.transform.position = target.transform.position + new Vector3(5, -0.98f, 0);
        target.setIsTarget(true);
    }
}
