using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    public string nickname { get; set; }

    private Animator animator;
   

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        
    }

    public void extractIdleNum()
    {
        int num = UnityEngine.Random.Range(0, 4);
        animator.SetInteger("idlePos", num);
    }

    public void IdleNumSetZero()
    {
        animator.SetInteger("idlePos", 0);
    }

    public void Ready()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
        animator.SetBool("isReady", true);
    }

    public void StartRace()
    {
       animator.SetBool("isStart", true);
    }

    public void setSpeed(float speed)
    {
        animator.SetFloat("speed", speed);
        //Debug.Log(animator.GetFloat("speed"));
    }

    public void setIsFallFlat(bool isFallFlat)
    {
        animator.SetBool("isFallFlat", isFallFlat);
    }

    public void setIsSideFallDown(bool isSideFallDown)
    {
        animator.SetBool("isSideFallDown", isSideFallDown);
    }

    public void setDropout(bool isDown)
    {
        animator.SetBool("Dropout", isDown);
    }

}
