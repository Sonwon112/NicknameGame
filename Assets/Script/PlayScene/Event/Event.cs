using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Event
{
    public void playEvent(CharacterMovement target);
    public Sprite getThumbnail();
    public string getEventName(); 
}
