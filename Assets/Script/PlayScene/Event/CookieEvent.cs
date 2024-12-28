using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieEvent : Event
{
    public PlayManager playManager;
    public GameObject Cookie;

    public override void playEvent(CharacterMovement target)
    {
        playManager.setAllTarget();
        target.setIsTarget(false);

        Instantiate(Cookie, target.transform);

    }

}
