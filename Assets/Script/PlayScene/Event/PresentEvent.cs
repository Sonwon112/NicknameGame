using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEvent : Event
{
    public GameObject Present;
    public override void playEvent(CharacterMovement target)
    {
        GameObject tmp = Instantiate(Present, target.transform);
        tmp.transform.localPosition = new Vector3(0.5f,0,0); 
    }
}
