using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseTrap : MonoBehaviour
{
    [SerializeField] private GameObject dirt;
    [SerializeField] private AudioSource destroySound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turtle Collider"))
        {
            Instantiate(dirt, transform.position, new Quaternion(-90,0,0,0));
            destroySound.Play();
            Destroy(this.gameObject);
           
        }
        
    }
}
