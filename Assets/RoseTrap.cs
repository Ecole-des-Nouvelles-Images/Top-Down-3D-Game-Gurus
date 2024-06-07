using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turtle"))
        {
            Destroy(this);
        }
    }
}
