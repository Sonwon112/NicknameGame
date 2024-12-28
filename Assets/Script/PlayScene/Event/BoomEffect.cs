using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            CharacterMovement tmp = other.GetComponent<CharacterMovement>();
            if (tmp == null) return;
            tmp.playDownAnim();
            Debug.Log("Down Player");
        }
    }
}
