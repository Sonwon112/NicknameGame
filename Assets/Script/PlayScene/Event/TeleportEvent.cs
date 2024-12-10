using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEvent : Event
{
    public GameObject Potal;
    public PlayManager PlayManager;

    private GameObject target1;
    private GameObject target2;
    private GameObject potal1;
    private GameObject potal2;
    private bool isTeleport = false;
    private double prevTime = 0f;
    private double currTime = 0f;
    private float teleportTerm = 3f;

    public override void playEvent(CharacterMovement target)
    {
        List<GameObject> list = PlayManager.getParticipantList();

        int targetIndex = UnityEngine.Random.Range(0, list.Count);

        target1 = target.gameObject;
        target2 = list[targetIndex].gameObject;
        potal1 = Instantiate(Potal, target.transform);
        potal2 = Instantiate(Potal, list[targetIndex].transform);
        potal1.transform.localPosition = potal2.transform.localPosition = new Vector3(0, -1, 0);

        isTeleport = true;
        prevTime = DateTime.Now.TimeOfDay.TotalSeconds;
    }

    private void Update()
    {
        if(isTeleport)
        {
            currTime = DateTime.Now.TimeOfDay.TotalSeconds;
            if(currTime - prevTime >= teleportTerm)
            {
                Vector3 tmpPos = target2.transform.position;
                target2.transform.position = target1.transform.position;
                target1.transform.position = tmpPos;
                isTeleport = false;
                Destroy(potal1);
                Destroy(potal2);
            }
        }
    }
}