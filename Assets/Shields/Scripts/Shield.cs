using System.Collections;
using UnityEngine;

namespace Shields.Scripts
{
    public class Shield : MonoBehaviour
    {
        Renderer _renderer;
        [SerializeField] AnimationCurve _DisplacementCurve;
        [SerializeField] float _DisplacementMagnitude;
        [SerializeField] float _LerpSpeed;
        [SerializeField] float _DisolveSpeed;
        [SerializeField] private float _autoCloseDelay = 2f;
        bool _shieldOn;
        bool _isAnimating;
        Coroutine _disolveCoroutine;

        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.SetFloat("_Disolve", 1);
            _isAnimating = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    HitShield(hit.point);
                }
            }
            if (Input.GetKeyDown(KeyCode.F) && !_shieldOn && !_isAnimating)
            {
                OpenShield();
            }
        }

        public void HitShield(Vector3 hitPos)
        {
            _renderer.material.SetVector("_HitPos", hitPos);
            StopAllCoroutines();
            StartCoroutine(Coroutine_HitDisplacement());
        }

        public void OpenShield()
        {
            if (!_isAnimating && !_shieldOn)
            {
                _shieldOn = true;
                _isAnimating = true; 
                if (_disolveCoroutine != null)
                {
                    StopCoroutine(_disolveCoroutine);
                }
                _disolveCoroutine = StartCoroutine(Coroutine_DisolveAndAutoClose(0)); 
            }
        }

        IEnumerator Coroutine_HitDisplacement()
        {
            float lerp = 0;
            while (lerp < 1)
            {
                _renderer.material.SetFloat("_DisplacementStrength", _DisplacementCurve.Evaluate(lerp) * _DisplacementMagnitude);
                lerp += Time.deltaTime * _LerpSpeed;
                yield return null;
            }
        }

        IEnumerator Coroutine_DisolveAndAutoClose(float target)
        {
            float start = _renderer.material.GetFloat("_Disolve");
            float lerp = 0;
            while (lerp < 1)
            {
                _renderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
                lerp += Time.deltaTime * _DisolveSpeed;
                yield return null;
            }
            _renderer.material.SetFloat("_Disolve", target);

            if (target == 0) 
            {
                yield return new WaitForSeconds(_autoCloseDelay);
                CloseShield(); 
            }
            else
            {
                _isAnimating = false; 
            }
        }

        private void CloseShield()
        {
            _shieldOn = false;
            _isAnimating = true; 
            if (_disolveCoroutine != null)
            {
                StopCoroutine(_disolveCoroutine);
            }
            _disolveCoroutine = StartCoroutine(Coroutine_DisolveAndAutoClose(1)); 
        }
    }
}
