using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    public List<GameObject> cookieList;
    [SerializeField] private float xBoundary = 10f;
    [SerializeField] private float yBoundary = 10f;
    [SerializeField] private int count = 5;
    [SerializeField] private float term = 1f;


    private float prevTime = 0;
    private float currTime = 0;
    private int currCnt = 0;

    private List<GameObject> tmpList = new List<GameObject>();

    private void Update()
    {
        if(currCnt < count)
        {
            currTime += Time.deltaTime;
            if (currTime - prevTime >= term)
            {
                int index = UnityEngine.Random.Range(0, cookieList.Count);
                float x = UnityEngine.Random.Range(0f, xBoundary);
                float y = UnityEngine.Random.Range(0f, yBoundary);

                int xSign = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                int ySign = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

                GameObject tmp = Instantiate(cookieList[index], transform);
                tmp.transform.localPosition = new Vector3(xSign * x, 10, ySign * y);
                tmp.transform.localRotation = Quaternion.Euler(40, 180, 0);
                tmpList.Add(tmp);

                prevTime = currTime;
                currCnt++;
            }
        }
        else
        {
            if(tmpList.Count <= 0)
            {
                Destroy(gameObject,1f);
            }
        }
    }

    public void removeList(GameObject cookie)
    {
        tmpList.Remove(cookie);
    }

}
