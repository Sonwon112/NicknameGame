using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    public GameObject ParticipantWindow;
    
    private ListContent currListContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenContent(string value)
    {
        GameObject gameObject = GameObject.Find(value);
        if(gameObject == null)
        {
            Debug.Log("�ش� �̸��� ������ ����� �������� �ʽ��ϴ�");
            return;
        }
        currListContent = gameObject.GetComponent<ListContent>();

    }

}
