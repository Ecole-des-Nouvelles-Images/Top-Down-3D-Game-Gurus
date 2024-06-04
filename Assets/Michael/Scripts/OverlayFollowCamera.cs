using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayFollowCamera : MonoBehaviour
{
   [SerializeField] private Camera mainCamera;
   [SerializeField] private Camera overlayCamera;


   private void Update()
   {
      overlayCamera.fieldOfView = mainCamera.fieldOfView;
   }
}
