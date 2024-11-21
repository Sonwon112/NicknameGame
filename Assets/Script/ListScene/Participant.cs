using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Participant : MonoBehaviour
{
    public int id { get; set; }
    public string nickname { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        TMP_Text txtNum = transform.Find("Num").gameObject.GetComponent<TMP_Text>();
        TMP_Text txtNick = transform.Find("Nickname").gameObject.GetComponent<TMP_Text>();

        txtNum.text = id+".";
        txtNick.text = nickname;
    }

}
