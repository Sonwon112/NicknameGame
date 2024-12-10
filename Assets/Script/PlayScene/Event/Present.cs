using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    public List<GameObject> presents = new List<GameObject>();
    public GameObject BoomParticle;
    public Transform DropBox;

    public float ShpereCastRadius = 10f;

    private GameObject present;
    private GameObject boomParticle;
    private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private float prevTime = 0f;
    private float currTime = 0f;
    private float term = 2f;
    private bool countDown = false;



    private void Start()
    {
        int index = UnityEngine.Random.Range(0, presents.Count);
        present = Instantiate(presents[index], DropBox);
    }

    private void Update()
    {
        if (countDown)
        {
            currTime = DateTime.Now.TimeOfDay.Seconds;
            if(currTime - prevTime >= term)
            {
                Destroy(present);
                boomParticle = Instantiate(BoomParticle,transform);
                countDown = false;

                RaycastHit[] targets = Physics.SphereCastAll(transform.position, ShpereCastRadius, Vector3.up,0);
                foreach(RaycastHit target in targets)
                {
                   
                    if (target.transform.gameObject.tag.Equals("Player"))
                    {
                        //Debug.Log(target.transform.name);
                        target.transform.gameObject.GetComponent<CharacterMovement>().playDownAnim();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("DropBox"))
        {
            prevTime = DateTime.Now.TimeOfDay.Seconds;
            countDown = true;
        }
    }


}
