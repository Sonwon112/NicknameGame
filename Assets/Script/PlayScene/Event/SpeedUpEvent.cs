using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEvent : MonoBehaviour, Event
{
    private CharacterMovement target;
    public Sprite thumbnail;
    public string eventName;
    public int targetcnt = 1;
    
    public void playEvent(CharacterMovement target)
    {
        this.target = target;
        this.target.speedUp();
    }

    public Sprite getThumbnail()
    {
        return thumbnail;
    }

    public string getEventName()
    {
        return eventName;
    }

}
