using UnityEngine;

namespace Noah.Scripts
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;

        [SerializeField] private AudioSource soundFXObject;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
        }
        
        public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
        {
            int rand = Random.Range(0, audioClip.Length);
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip[rand];
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
        }
    }
}
