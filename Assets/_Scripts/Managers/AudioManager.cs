using UnityEngine;

namespace _Scripts.Managers {
    public class AudioManager : MonoBehaviour {

        private static AudioManager _instance = null;

        public enum AudioSourceName { MusicSource, SfxSource }

        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource sfxSource;

        public static AudioManager Instance { get { return _instance; } }

        public AudioSource MusicSource {
            get { return musicSource; }
        }

        public AudioSource SfxSource {
            get { return sfxSource; }
        }

        private void Awake () {
            EnsureSingleton ();
        }

        private void EnsureSingleton () {
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                Destroy (gameObject);
            }
        }

        public void PlayAudioClip (AudioSourceName audioSourceName, AudioClip audioClip, bool overwriteActualAudio) {
            GetRequestedAudioSource (audioSourceName, out AudioSource actualSrc);
            if (overwriteActualAudio || !actualSrc.isPlaying) {
                actualSrc.clip = audioClip;
                actualSrc.Play ();
            }
        }

        public void ResumeAudio (AudioSourceName audioSourceName) {
            GetRequestedAudioSource (audioSourceName, out AudioSource actualSrc);
            actualSrc.Play();
        }

        public void StopAudio (AudioSourceName audioSourceName) {
            GetRequestedAudioSource (audioSourceName, out AudioSource actualSrc);
            actualSrc.Stop();
        }

        public void UpdateAudioSourceVolume (AudioSourceName audioSourceName, float newVolume) {
            GetRequestedAudioSource (audioSourceName, out AudioSource actualSrc);
            actualSrc.volume = newVolume;
        }

        // public void DetermineAudioVolume ()

        private void GetRequestedAudioSource (AudioSourceName audioSourceName, out AudioSource actualSrc) {
            switch (audioSourceName) {
                case AudioSourceName.MusicSource:
                    actualSrc = musicSource;
                    break;

                case AudioSourceName.SfxSource:
                    actualSrc = sfxSource;
                    break;

                default:
                    actualSrc = musicSource;
                    break;
            }
        }
    }
}