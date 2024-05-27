using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public class Obstacle : MonoBehaviour
{
   [SerializeField] private float shakeThreshold = 5f;
   [SerializeField] private Vector3 shakeStrengh = new Vector3(0.01f,0,0.01f);
   [SerializeField] private int shakeVibrato = 1;
   [SerializeField] private List<GameObject> Tomatoe;
   [SerializeField] private ParticleSystem waterRipples;
   [SerializeField] private GameObject brokenPot;
   void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Turtle")) {
            Rigidbody turtleRb = collision.gameObject.GetComponent<Rigidbody>();
            if (turtleRb.velocity.magnitude > shakeThreshold) {
                ShakeObject();
                if (Tomatoe.Count != 0) {
                    int randomTomatoe = Random.Range(0, Tomatoe.Count-1);
                    Tomatoe[randomTomatoe].GetComponent<Rigidbody>().isKinematic = false;
                }

                if (waterRipples) {
                   waterRipples.Play(); 
                }

                if (brokenPot)
                { 
                    Instantiate(brokenPot, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                
            }
        }
    }

    [ContextMenu("ShakeObject")]
    private void ShakeObject()
    {
        gameObject.transform.DOShakePosition(1f, shakeStrengh, shakeVibrato);
    }
}
