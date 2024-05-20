using System;
using UnityEngine;

namespace Noah.Scripts
{
    public class GrapplingGun : MonoBehaviour
    {
        private LineRenderer lr;
        private Vector3 grapplePoint;
        public LayerMask whatIsGrappleable;
        public Transform GunTip, Player;
        [SerializeField] private float _maxDistance = 5f;
        private SpringJoint joint;

        void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }
        

        private void Update()
        {
            DrawRope();
            if (Input.GetMouseButtonDown(0))
            {
                StartGrapple();
            }
            
            else if (Input.GetMouseButtonUp(0))
            {
                StopGrapple();
            }
        }

        private void LateUpdate()
        {
            DrawRope();
        }

        private void StartGrapple()
        {
            RaycastHit hit;
            if (Physics.Raycast(GunTip.position, GunTip.forward, out hit, _maxDistance, whatIsGrappleable))
            {
                grapplePoint = hit.point;
                joint = Player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(Player.position, grapplePoint);

                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;
            }
        }

        void DrawRope()
        {
            lr.SetPosition(0, GunTip.position);
            lr.SetPosition(0, grapplePoint);
        }
        
        private void StopGrapple()
        {
            throw new NotImplementedException();
        }
    }
}
