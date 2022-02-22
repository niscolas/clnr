using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Controllers.Menus {
    public class LevelSelectorMenuController : MonoBehaviour {

        [SerializeField]
        private Button beginBtt;

        [SerializeField]
        private Image levelImgHolder;

        [SerializeField]
        private TextMeshProUGUI descriptionTxtHolder;

        [SerializeField]
        private TextMeshProUGUI usefulInfoTxtHolder;

        private string levelToLoad = "";

        [Serializable]
        private class LoadSceneConfig {
            [SerializeField]
            internal Sprite levelImg;

            [SerializeField]
            internal string descriptionTxt;

            [SerializeField]
            internal string levelToLoad;

            [SerializeField]
            internal string usefulInfoTxt;
        }

        [SerializeField]
        private LoadSceneConfig[] loadSceneConfigs;

        private LoadSceneConfig actualConfig;

        private void Awake () {
            beginBtt.onClick.AddListener (LoadLevel);
        }

        public void UpdateInterface (int configIndex) {
            if (configIndex < loadSceneConfigs.Length) {
                actualConfig = loadSceneConfigs[configIndex];

                levelImgHolder.sprite = actualConfig.levelImg;
                descriptionTxtHolder.text = actualConfig.descriptionTxt;
                usefulInfoTxtHolder.text = actualConfig.usefulInfoTxt;
                this.levelToLoad = actualConfig.levelToLoad;
            }
        }

        private void LoadLevel () {
            if (levelToLoad != "") {
                SceneController.LoadSceneByName (levelToLoad);
            }
        }
    }
}