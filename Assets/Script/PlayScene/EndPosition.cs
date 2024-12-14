using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EndPosition : MonoBehaviour
{
    public PlayManager playManager;
    private List<GameObject> PlayerList = new List<GameObject>();
    private List<string> RankingArr = new List<string>();

    private void Start()
    {
        

    }

    private void Update()
    {
        if (playManager.isStart)
        {
            PlayerList = GameObject.FindGameObjectsWithTag("Player").ToList();
            PlayerList = PlayerList.OrderBy(obj => this.transform.position.x - obj.transform.position.x).ToList();
            RankingArr.Clear();
            int tmp = PlayerList.Count < 10 ? PlayerList.Count : 10;
            for (int i = 0; i < tmp; i++)
            {
                RankingArr.Add(PlayerList[i].GetComponent<CharacterMovement>().NicknameText.text);
            }
            if (tmp != 0)
            {
                playManager.setRankingList(RankingArr.ToList());
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CharacterMovement participant = other.GetComponent<CharacterMovement>();
            playManager.AppendRank(participant);
            
        }
        if(other.tag.Equals("Santa"))
        {
            Destroy(other.gameObject);
        }
    }
}
