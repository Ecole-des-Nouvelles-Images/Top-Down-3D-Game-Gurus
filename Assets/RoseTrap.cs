using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseTrap : MonoBehaviour
{
    [SerializeField] private GameObject dirt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turtle"))
        {
            Instantiate(dirt, transform.position, transform.rotation);
            Destroy(this);
        }
        
        
    }
}
