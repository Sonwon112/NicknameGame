using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPosition : MonoBehaviour
{
    public PlayManager PlayManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Participant participant = other.GetComponent<Participant>();
            PlayManager.AppendRank(participant);
            
        }   
    }
}
