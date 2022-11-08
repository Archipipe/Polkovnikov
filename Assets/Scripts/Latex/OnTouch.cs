using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouch : MonoBehaviour
{
    AudioSource pickUpSound;
    Material dissolve;
    new Collider collider;
    bool isDissolving;
    float f = 1;

    private void Awake()
    {
        pickUpSound = GetComponentInChildren<AudioSource>();
        dissolve = GetComponent<Renderer>().material;
        collider = GetComponent<Collider>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            isDissolving = true;
            collider.enabled = false;
            pickUpSound.Play();
        }
    }

    private void Update()
    {
        if (isDissolving)
        {
            f = Mathf.MoveTowards(f,-1,Time.deltaTime * 2f);
            dissolve.SetFloat("_Float", f);
            if (f == -1) Destroy(gameObject); 
        }
    }
}
