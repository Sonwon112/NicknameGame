using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkEvent : Event
{
    public GameObject Fork;

    public override void playEvent(CharacterMovement target)
    {
        GameObject tmp = Instantiate(Fork, target.transform);
        target.setIsTarget(true);
    }
}
