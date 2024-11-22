using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvnetZone : MonoBehaviour
{
    public PlayManager playManager;


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playManager.DrawEvent();
        }
    }
}
