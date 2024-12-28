using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideEventCount : MonoBehaviour
{
    [SerializeField] private int min;
    [SerializeField] private int max;
    GameObject[] eventZoneList;

    // Start is called before the first frame update
    void Start()
    {
        eventZoneList = GameObject.FindGameObjectsWithTag("EventZone");
        int cnt = Random.Range(min, max);
        for(int i = 0; i < cnt; i++)
        {
            int index = Random.Range(0, eventZoneList.Length-1);
            eventZoneList[index].GetComponent<BoxCollider>().enabled = true;
        }
    }
}
