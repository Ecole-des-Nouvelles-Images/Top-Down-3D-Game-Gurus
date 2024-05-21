using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Noah.Scripts
{
    public class PoppyController : FlowerController
    {
        [Header("References")]
        public Transform Gun;
        public LayerMask whatIsGrappleable;
        public LineRenderer lr;

        [Header("Grappling")]
        public float grappleSpeed;
        public float maxGrappleDistance;
        public float grappleMaxHoldTime = 2f;

        [Header("Cooldown")]
        public float grapplingCd;
        private float _grapplingCdTimer;
        
        private float _grappleHoldTimer;
        private Vector3 grapplePoint;
        private bool _isgrappling;
        private bool _shouldApplyGrappleForce;

         [SerializeField] private float grapplingSpeed;

        protected override void Start()
        {
            base.Start();
            lr.enabled = false;
            lr.positionCount = 2;
        }

        protected override void PassiveCapacity()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            base.Update();
            GrapplingUpdate();
        }

        private void GrapplingUpdate()
        {
            if (_grapplingCdTimer > 0)
                _grapplingCdTimer -= Time.deltaTime;

            if (_isgrappling)
            {
                HoldGrapple();
            }
        }

        protected override void FixedUpdate()
        {
            if (!_isgrappling)
            {
                Move();
            }
            else if (_shouldApplyGrappleForce)
            {
                ApplyGrappleForce();
            }
        }

        public override void OnMainCapacity(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                MainCapacity();
            }

            if (context.canceled)
            {
                ExecuteGrapple();
            }
        }

        protected override void MainCapacity()
        {
            StartGrapple();
        }

        private void StartGrapple()
        {
            if (_grapplingCdTimer > 0) return;
            _grappleHoldTimer = 0f;
            _isgrappling = true;
            lr.enabled = true;
            Debug.Log("Started Grapple");
        }

        private void HoldGrapple()
        {
            if (!_isgrappling) return;

            _grappleHoldTimer += Time.deltaTime;
            _grappleHoldTimer = Mathf.Min(_grappleHoldTimer, grappleMaxHoldTime);

            if (grappleMaxHoldTime > 0 && !_shouldApplyGrappleForce)
            {
                // currentGrappleDistance = maxGrappleDistance * ((_grappleHoldTimer * _grappleMultiplier) / grappleMaxHoldTime);
                float currentGrappleForce = Mathf.Clamp(grapplingSpeed * _grappleHoldTimer , 0f, maxGrappleDistance);
                Debug.Log(grapplingSpeed * _grappleHoldTimer);
                
                lr.SetPosition(0, Gun.position);
                lr.SetPosition(1, Gun.position + Gun.forward * currentGrappleForce);
                
                RaycastHit hit;
                if (Physics.Raycast(Gun.position, Gun.forward, out hit, currentGrappleForce, whatIsGrappleable))
                {
                    grapplePoint = hit.point;
                    _shouldApplyGrappleForce = true;

                    lr.SetPosition(0, Gun.position);
                    lr.SetPosition(1, grapplePoint);
                    

                    _grapplingCdTimer = grapplingCd;
                }
            }
        }

        private void ExecuteGrapple()
        {
            if (!_isgrappling) return;

            if (grappleMaxHoldTime > 0)
            {
                StopGrapple();
            }
            else
            {
                StopGrapple();
            }
        }

        private void ApplyGrappleForce()
        {
            Vector3 directionToGrapple = (grapplePoint - transform.position).normalized;
            Vector3 forceVector = directionToGrapple * grappleSpeed * Time.deltaTime;

            Rb.AddForce(forceVector, ForceMode.Force);

            lr.SetPosition(0, Gun.position);
            lr.SetPosition(1, grapplePoint);

            if (Vector3.Distance(transform.position, grapplePoint) < 0.1f)
            {
                StopGrapple();
            }
        }

        private void StopGrapple()
        {
            _isgrappling = false;
            _shouldApplyGrappleForce = false;
            _grapplingCdTimer = grapplingCd;
            lr.enabled = false;
            Debug.Log("Stopped Grapple");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != null)
            {
                StopGrapple();
            }
        }

        private void OnDrawGizmos()
        {
            if (grapplePoint != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(grapplePoint, 0.1f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Gun.position, maxGrappleDistance);
        }
    }
}
