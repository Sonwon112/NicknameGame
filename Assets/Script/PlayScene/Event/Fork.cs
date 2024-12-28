using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : MonoBehaviour
{
    public GameObject particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameObject tmp = Instantiate(particle);
            tmp.transform.position = transform.position;
            Destroy(tmp, 1f);
        }
    }
}
