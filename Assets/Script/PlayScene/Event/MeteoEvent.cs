using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoEvent : Event
{
    public GameObject Meteor;
    public override void playEvent(CharacterMovement target)
    {
        GameObject tmp = Instantiate(Meteor);
        target.setIsTarget(true);
        tmp.transform.position = target.transform.position + new Vector3(30, 15, 0);
        tmp.GetComponent<Meteor>().setTargetPlayer(target.gameObject);
    }
}
