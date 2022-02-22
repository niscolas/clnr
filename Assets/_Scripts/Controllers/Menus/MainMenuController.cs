using System.IO;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Controllers.Menus {
	public class MainMenuController : MonoBehaviour {
		[SerializeField]
		private string musicsFolderPath = "Audio/Music/";

		private AudioClip menuMusic;
		private AudioManager audioManager;

		private void Awake() {
			Debug.Log("a");
			
			DirectoryInfo musicDirInfo = new DirectoryInfo("Assets/Resources/" + musicsFolderPath);
			FileInfo[] musicFilesInfos = musicDirInfo.GetFiles("*.mp3?");

			int chosenIndex = Random.Range(0, musicFilesInfos.Length);

			FileInfo chosenMusicFileInfo = musicFilesInfos[chosenIndex];
			string finalMusicPath = musicsFolderPath + "/" + chosenMusicFileInfo.Name.Replace(".mp3", "");

			Object music = Resources.Load(
				finalMusicPath,
				typeof(AudioClip));

			menuMusic = music as AudioClip;
			
			Debug.Log("b");
		}

		private void Start() {
			Debug.Log("c");
			
			audioManager = AudioManager.Instance;

			audioManager.PlayAudioClip(
				AudioManager.AudioSourceName.MusicSource,
				menuMusic,
				true);

			Debug.Log("AM: -> Now playing: " + menuMusic.name);
			
			Debug.Log("d");
		}
	}
}