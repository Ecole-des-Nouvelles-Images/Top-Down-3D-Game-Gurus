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
        public float grappleMaxHoldTime = 2f; 

        [Header("Cooldown")]
        public float grapplingCd;
        private float _grapplingCdTimer;

        public float _grappleMultiplier = 1;

        private float _grappleHoldTimer;
        private Vector3 grapplePoint;
        private bool _isgrappling;
        private bool _shouldApplyGrappleForce;

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
                StartGrapple();
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
            lr.SetPosition(0, gunTip.position); // Set initial positions
            lr.SetPosition(1, gunTip.position); // Set initial positions
            Debug.Log("Started Grapple");
        }

        private void HoldGrapple()
        {
            if (!_isgrappling) return;

            _grappleHoldTimer += Time.deltaTime;
            _grappleHoldTimer = Mathf.Min(_grappleHoldTimer, grappleMaxHoldTime);

            if (grappleMaxHoldTime > 0)
            {
                float currentGrappleDistance = maxGrappleDistance * ((_grappleHoldTimer * _grappleMultiplier) / grappleMaxHoldTime);
                lr.SetPosition(0, gunTip.position);
                lr.SetPosition(1, gunTip.position + gunTip.forward * currentGrappleDistance);
            }
        }

        private void ExecuteGrapple()
        {
            if (!_isgrappling) return;

            if (grappleMaxHoldTime > 0)
            {
                float currentGrappleDistance = maxGrappleDistance * ((_grappleHoldTimer * _grappleMultiplier) / grappleMaxHoldTime);

                RaycastHit hit;
                if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, currentGrappleDistance, whatIsGrappleable))
                {
                    grapplePoint = hit.point;
                    _shouldApplyGrappleForce = true;

                    lr.SetPosition(0, gunTip.position);
                    lr.SetPosition(1, grapplePoint);

                    float distanceToGrapple = Vector3.Distance(gunTip.position, grapplePoint);

                    float timeToReachGrapple = distanceToGrapple / grappleSpeed;

                    float elapsedTime = 0f;
                    Vector3 startPosition = gunTip.position;

                    while (elapsedTime < timeToReachGrapple)
                    {
                        elapsedTime += Time.deltaTime;
                        float t = elapsedTime / timeToReachGrapple;
                        lr.SetPosition(0, Vector3.Lerp(startPosition, grapplePoint, t));
                    }
                }
                else
                {
                    _isgrappling = false;
                    lr.enabled = false;
                }
            }
            else
            {
                _isgrappling = false;
                lr.enabled = false;
            }

            _grapplingCdTimer = grapplingCd;
        }

        private void ApplyGrappleForce()
        {
            Vector3 directionToGrapple = (grapplePoint - transform.position).normalized;
            Vector3 forceVector = directionToGrapple * grappleSpeed * Time.deltaTime;

            Rb.AddForce(forceVector, ForceMode.Force);

            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);

            if (Vector3.Distance(transform.position, grapplePoint) < 0.1f)
            {
                _shouldApplyGrappleForce = false;
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
            // Draw a line for the current grapple distance while holding
            if (_isgrappling)
            {
                float currentGrappleDistance = maxGrappleDistance * ((_grappleHoldTimer * _grappleMultiplier) / grappleMaxHoldTime);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(gunTip.position, gunTip.position + gunTip.forward * currentGrappleDistance);
            }
            // Draw a sphere at the grapple point when grappled
            if (grapplePoint != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(grapplePoint, 0.1f);
            }

            // Draw the max grapple distance
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gunTip.position, maxGrappleDistance);
        }
    }
}


