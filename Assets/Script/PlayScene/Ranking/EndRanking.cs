using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndRanking : MonoBehaviour
{
    public GameObject ContentWrapper;
    public GameObject RankListContent;

    public void setRankList(List<string> rankList)
    {
        for(int i = 0; i < rankList.Count; i++)
        {
            GameObject tmp = Instantiate(RankListContent, ContentWrapper.transform);

            tmp.GetComponent<Participant>().id = i + 1;
            tmp.GetComponent<Participant>().nickname = rankList[i];
        }
    }
}
