using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulesRotation : MonoBehaviour
{
    [SerializeField] private Transform Parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Parent.rotation;
    }
}
