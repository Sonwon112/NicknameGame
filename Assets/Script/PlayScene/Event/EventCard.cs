using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EventCard : MonoBehaviour
{
    public List<Event> eventsList = new List<Event>();
    public GameObject TargetText;

    private Animator m_Animator;
    private Image m_Thumbnail;
    private TMP_Text m_Text;
    private List<GameObject> targets = new List<GameObject>();
    private bool callDraw = false;

    private Event currEvent;
    private CharacterMovement currTarget;
    private int randomIndex;

    private int type = 0; // 0 event 1 target
    // 뽑기 효과용 변수
    private int currCount = 0;
    private int index = 0;
    private int[] countArr = {10, 18, 23, 26};
    private float[] delayArr = { 1, 2, 3, 4};
    private float prevTime;
    private float currTime;

    private bool isNicknameDispaly = false;
    private int displayNicknameDelay = 3;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Thumbnail = transform.Find("Thumbnail").gameObject.GetComponent<Image>();
        m_Text = transform.Find("EventText").gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(callDraw)
        {
            currCount++;
            if (currCount > countArr[index]) index++;
            if(index >  countArr.Length)
            {
                callDraw = false;
                if (type == 0) { DisplayCard(); }
                else
                {
                    isNicknameDispaly = true;
                    prevTime = currTime = 0;
                }
                return;
            }
            currTime += Time.unscaledDeltaTime;
            if(currTime-prevTime > delayArr[index])
            {
                prevTime = currTime;
                int tmp = type == 0 ? eventsList.Count : targets.Count;
                randomIndex = UnityEngine.Random.Range(0, tmp);
                switch(type)
                {
                    case 0:
                        currEvent = eventsList[randomIndex];
                        m_Thumbnail.sprite = currEvent.getThumbnail();
                        m_Text.text = currEvent.getEventName();
                        break;
                    case 1:
                        currTarget = targets[randomIndex].GetComponent<CharacterMovement>();
                        TargetText.GetComponent<TMP_Text>().text = currTarget.NicknameText.text;
                        break;
                }
            }
        }

        if(isNicknameDispaly) {
            currTime += Time.unscaledDeltaTime;
            if (currTime - prevTime >= displayNicknameDelay)
            {
                isNicknameDispaly = false;
                TargetText.SetActive(false);
                TargetText.GetComponent<TMP_Text>().text = "";
                currEvent.playEvent();
            }
        }

    }

    public void OpenCard(List<GameObject> targets)
    {
        m_Animator.SetInteger("state", 0);
        this.targets = targets;
    }

    public void DisplayCard()
    {
        type = 1;
        m_Animator.SetInteger("state", 1);
    }

    public void drawEvent()
    {
        callDraw = true;
        type = 0;
        ResetEffectVar();
    }

    public void drawTarget()
    {
        callDraw = true;
        TargetText.SetActive(true);
        type = 1;
        ResetEffectVar();
    }

    public void ResetEffectVar()
    {
        index = 0;
        currCount = 0;
        prevTime = 0;
        currTime = 0;
    }
   
}
