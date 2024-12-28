using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEvent : Event
{
    public override void playEvent(CharacterMovement target)
    {
        this.target = target;
        Debug.Log("PlaySampleEvent : " + eventName);
    }
}
