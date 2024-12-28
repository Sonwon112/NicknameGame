using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballEvent : Event
{
    public GameObject snowBall;
    public GameObject shield;
    public PlayManager manager;
    public override void playEvent(CharacterMovement target)
    {
        GameObject tmp = Instantiate(snowBall);
        tmp.transform.position = new Vector3(target.transform.position.x + 50, 0, 0);
        tmp.transform.rotation = Quaternion.Euler(0,-90,0);
        manager.setAllTarget();
        GameObject shieldTmp = Instantiate(shield,target.transform);
        shieldTmp.transform.localPosition = new Vector3(0.9f,0,0);
        shieldTmp.transform.rotation = Quaternion.Euler(0, 90, 0);
        target.setIsShield(true,shieldTmp);

    }
}
