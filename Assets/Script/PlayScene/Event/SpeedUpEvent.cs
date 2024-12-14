using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEvent : Event
{   
    public override void playEvent(CharacterMovement target)
    {
        this.target = target;
        this.target.speedUp();
    }
}
