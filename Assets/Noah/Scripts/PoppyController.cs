using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah.Scripts
{
    public class PoppyController : FlowerController
    {
        [Header("References")]
        public Transform gunTip;
        public LayerMask whatIsGrappleable;
        public LineRenderer lr;

        [Header("Grappling")]
        public float grappleSpeed;
        public float maxGrappleDistance;
        public float grappleDelayTime;
        
        [Header("Cooldown")]
        public float grapplingCd;
        private float _grapplingCdTimer;
        
        private Vector3 grapplePoint;
        private bool _grappling;

        protected override void Start()
        {
            base.Start();
            lr.enabled = false;
        }
        
        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        } 

        protected override void Update()
        {
            if (_grapplingCdTimer > 0)
                _grapplingCdTimer -= Time.deltaTime;
        }
        
        public override void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                MainCapacity();
            }
        }

        protected override void MainCapacity()
        {
            StartGrapple();
        }

        protected override void FixedUpdate()
        {
            if (!_grappling)
            {
                Move();
            }
        }

        private void OnDrawGizmos()
        {
            if (_grapplingCdTimer > 0) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gunTip.position, gunTip.position + gunTip.forward * maxGrappleDistance);
        }
        
        private void StartGrapple()
        {
            if (_grapplingCdTimer > 0) return;

            _grappling = true;

            RaycastHit hit;
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, maxGrappleDistance, whatIsGrappleable))
            {
                grapplePoint = hit.point;
                lr.enabled = true;
                lr.positionCount = 2;
                lr.SetPosition(0, gunTip.position);
                lr.SetPosition(1, grapplePoint);
                lr.enabled = true;
                ExecuteGrapple();
            }
            else
            {
                Debug.Log("Nowhere to land");
                StopGrapple();
            }
        }


        private void ExecuteGrapple()
        {
            Vector3 directionToGrapple = (grapplePoint - transform.position).normalized;
            Vector3 forceVector = directionToGrapple * grappleSpeed;
            Rb.AddForce(forceVector * Time.deltaTime, ForceMode.Force);

            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);

            if (Vector3.Distance(transform.position, grapplePoint) < 0.3f)
            {
                StopGrapple();
            }
            else
            {
                Invoke(nameof(ExecuteGrapple), Time.fixedDeltaTime);
            }
        }

        public void StopGrapple()
        {
            _grappling = false;
            _grapplingCdTimer = grapplingCd;
            lr.enabled = false;
        }

        public bool IsGrappling()
        {
            return _grappling;
        }

        public Vector3 GetGrapplePoint()
        {
            return grapplePoint;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != null) 
            {
                StopGrapple();
            }        
        }
    }
}
