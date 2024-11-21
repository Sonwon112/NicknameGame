using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private bool isStart = false;
    private float currTime;
    private float prevTime;
    private TMP_Text countText;

    private int latency = 3;

    // Start is called before the first frame update
    void Start()
    {
        countText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
           
            float currCount = Time.time-prevTime;
            int curr = latency-(int)currCount;
            if (curr == -1)
            {
                isStart = false;
                gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayManager>().raceStart();
                return;
            } 
            countText.text = curr <= 0 ? "Go!" :""+curr;
        }
    }

    public void CountStart()
    {
        isStart = true;
        prevTime = Time.time;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayManager>().raceReady();
    }
}
