using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieInstance : MonoBehaviour
{

    private float time = 0f;
    private bool flag = false;
    public void CallDestroy()
    {
        if(transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            parent.GetComponent<Cookie>().removeList(gameObject);
        }
        
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            parent.GetComponent<Cookie>().removeList(gameObject);
            transform.parent = null;
        }        
        Destroy(gameObject,1f);
    }
}
