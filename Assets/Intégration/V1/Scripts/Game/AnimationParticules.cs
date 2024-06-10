using UnityEngine;

namespace Intégration.V1.Scripts.Game
{
    public class AnimationParticules : MonoBehaviour
    {
        [SerializeField] private GameObject _crashParticules;
        [SerializeField] private ParticleSystem _dirtParticules;
        [SerializeField] private ParticleSystem _runParticules;
        private Animator _animator;
        [SerializeField] private AudioSource footstepsound;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShowCrashParticules()
        {
            if (_crashParticules)
            {
                _crashParticules.SetActive(true);
            }
        }

        public void ShowDirtParticules()
        {
            if (_dirtParticules)
            {
                _dirtParticules.Play();
            }
        }

        public void RunParticules()
        {
            if (_runParticules)
            {
                _runParticules.Play();
                footstepsound.Play();
            }
        }


        public void DestroyDirt()
        {
            Destroy(gameObject);
        }
    }
}