using _Scripts.Managers;
using UnityEngine;
using static _Scripts.Managers.AudioManager.AudioSourceName;

namespace _Scripts.UI {
    public class SettingsMenuController : MonoBehaviour {

        public void UpdateMusicVolume (float actualPercentage) {
            AudioManager.Instance.UpdateAudioSourceVolume (MusicSource, actualPercentage);
        }

        public void UpdateSfxVolume (float actualPercentage) {
            AudioManager.Instance.UpdateAudioSourceVolume (SfxSource, actualPercentage);
        }
    }
}