using System.Collections;
using UnityEngine;

namespace _Scripts.Utils.UI.Image {

	public class ImageFader : MonoBehaviour {
		private static ImageFader _instance = null;

		/// <summary>
		/// 	A simple Enum to limit to determine the Fade Speed
		/// </summary>
		public enum FadeSpeed {
			SLOW = 4, NORMAL = 8, FAST = 16
		}
		private FadeSpeed fadeSpeed;

		/// <summary>
		/// 	Defines how slow the image will Fade
		/// </summary>
		private const float baseSpeedDivider = 16f;

		// Using Singleton Pattern
		private ImageFader () { }
		// 
		public static ImageFader Instance {
			get {
				if (_instance == null) {
					GameObject container = new GameObject ("ImageFader");
					_instance = container.AddComponent<ImageFader> ();
				}
				return _instance;
			}
		}

		/// <summary>
		///     This method creates a animation of Fade In and Fade Out. 
		///     It consists in three fases:
		///         - Firstly the image will start with Alpha = 0, gradually Fading In (gaining opacity)
		///			- Then, the image will remain on screen
		/// 		- Finally, it will Fade Out
		/// </summary>
		/// <param name="img"> The Image used on the Fade Cycle </param>
		/// <param name="fadeSpeed"> The speed which the image will Fade In, be on screen and then Fade Out </param>
		public IEnumerator CycleFade (UnityEngine.UI.Image img, FadeSpeed fadeSpeed) {
			yield return StartCoroutine (FadeImageCoroutine (img, false, fadeSpeed));
			yield return new WaitForSeconds ((int) fadeSpeed / (baseSpeedDivider / 2));
			yield return StartCoroutine (FadeImageCoroutine (img, true, fadeSpeed));
		}

		/// <summary>
		///     Fades the given image in or out.
		/// </summary>
		/// <param name="img"> The image to fade </param>
		/// <param name="shouldFadeOut"> Whether the image should fade in or out </param>
		/// <param name="speedMultiplier"> The speed which the image will fade </param>
		public IEnumerator FadeImageCoroutine (UnityEngine.UI.Image img, bool shouldFadeOut, FadeSpeed fadeSpeed) {
			this.fadeSpeed = fadeSpeed;

			// Fade the image out (turns into transparent)
			if (shouldFadeOut) {
				for (float i = 1; i >= 0; i -= GetProcessedDeltaTime ()) {
					img.color = new Color (1, 1, 1, i);
					yield return null;
				}
			}
			// Fade the image out (turns into opaque)
			else {
				for (float i = 0; i <= 1; i += GetProcessedDeltaTime ()) {
					img.color = new Color (1, 1, 1, i);
					yield return null;
				}
			}
		}

		private float GetProcessedDeltaTime () {
			return Time.deltaTime / baseSpeedDivider * (int) fadeSpeed;
		}
	}
}