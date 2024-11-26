using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EventCard : MonoBehaviour
{
    public List<GameObject> eventsList = new List<GameObject>();
    public GameObject TargetText;
    public ParticleSystem Fanfare;

    private Animator m_Animator;
    private Image m_Thumbnail;
    private TMP_Text m_Text;
    private List<GameObject> targets = new List<GameObject>();
    private bool callDraw = false;

    private Event currEvent;
    private CharacterMovement currTarget;
    private int targetIndex;
    private int eventIndex;

    private int type = 0; // 0 event 1 target
    // 뽑기 효과용 변수
    private int currCount = 0;
    private int index = 0;
    private int[] countArr = { 15, 18, 22, 25 };
    private float[] delayArr = { 0.1f, 0.15f, 0.3f, 0.5f };
    private float prevTime;
    private float currTime;

    private bool isNicknameDispaly = false;
    private int displayNicknameDelay = 3;
    private const int DEFAULT_FONTSIZE = 80;
    private const int HIGHLIGHT_FONTSIZE = 100;

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
        if (callDraw)
        {

            currTime += Time.unscaledDeltaTime;
            if (currTime - prevTime > delayArr[index])
            {
                currCount++;
                if (currCount > countArr[index]) index++;
                if (index >= countArr.Length)
                {
                    callDraw = false;
                    DisplayCard();
                    return;
                }

                prevTime = currTime;
                targetIndex = UnityEngine.Random.Range(0, targets.Count);
                eventIndex = UnityEngine.Random.Range(0, eventsList.Count);

                currEvent = eventsList[eventIndex].GetComponent<Event>();
                m_Thumbnail.sprite = currEvent.getThumbnail();
                m_Text.text = currEvent.getEventName();

                currTarget = targets[targetIndex].GetComponent<CharacterMovement>();
                TargetText.GetComponent<TMP_Text>().text = currTarget.NicknameText.text;
            }
        }
    }

    public void PlayFanFare()
    {
        Fanfare.Play();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        currEvent.playEvent(currTarget);
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
