using UnityEngine;
using UnityEngine.Audio;

namespace Noah.Scripts
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void SetMasterVolume(float level)
        {
            audioMixer.SetFloat("masterVolume", level);
        }
        
        public void SetSoundFXVolume(float level)
        {
            audioMixer.SetFloat("soundFX", level);
        }
        
        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat("musicVolume", level);
        }
    }
}
