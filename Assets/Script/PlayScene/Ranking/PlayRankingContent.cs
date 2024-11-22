using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayRankingContent : MonoBehaviour
{
    private string nickname = "";
    private TMP_Text nicknameText;

    private void Start()
    {
        GameObject nicknameTextObj = transform.Find("nickname").gameObject;
        nicknameText = nicknameTextObj.GetComponent<TMP_Text>();
    }

    public void setNickname(string nickname)
    {
        this.nickname = nickname;
        nicknameText.text = nickname;
    }

}
