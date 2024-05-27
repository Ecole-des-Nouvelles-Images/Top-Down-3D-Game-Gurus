using System.Collections;
using System.Collections.Generic;
using Michael.Scripts.Manager;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
  public void StartSlowMotion()
  {
    GameManager.Instance.StartSlomotion();
    GameManager.Instance.CameraShake(0.5f,0.1f,10);
  }
  
  public void StopSlowMotion()
  {
    GameManager.Instance.FinishSlomotion();
  }
}
