using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    protected CharacterMovement target;

    public Sprite thumbnail;
    public string eventName;
    public int targetcnt = 1;
    public abstract void playEvent(CharacterMovement target);
    public Sprite getThumbnail() { return thumbnail; }
    public string getEventName() { return eventName; } 
}
