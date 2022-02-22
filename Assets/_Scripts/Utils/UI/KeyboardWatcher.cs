using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Utils.UI {
    public class KeyboardWatcher : MonoBehaviour {

        public List<KeyCode> pressedKeys {get; set;}
        private int[] values;
        private bool[] keys;

        void Awake () {
            foreach (var keycode in Enum.GetValues(typeof(KeyCode))) {
                Debug.Log("KeyCode: " + keycode + "; As Int: " + (int) keycode);
            }

            values = (int[]) System.Enum.GetValues (typeof (KeyCode));
            keys = new bool[values.Length];
        }

        void Update () {
            pressedKeys.Clear();
            for (int i = 0; i < values.Length; i++) {
                KeyCode actualKey = (KeyCode) values[i];
                if (Input.GetKey (actualKey) == true) {
                    pressedKeys.Add(actualKey);
                }
            }
        }
    }
}