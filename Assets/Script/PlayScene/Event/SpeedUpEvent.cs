using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEvent : Event
{
    public GameObject effect;
    public override void playEvent(CharacterMovement target)
    {
        this.target = target;
        this.target.speedUp();
        GameObject tmp = Instantiate(effect, target.transform);
        tmp.transform.localPosition = new Vector3(0, -1f,0);

        Destroy(tmp, 3f);
    }
}
