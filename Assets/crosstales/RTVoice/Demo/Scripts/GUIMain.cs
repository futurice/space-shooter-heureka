using UnityEngine;
using UnityEngine.UI;
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
using UnityEngine.SceneManagement;
#endif
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Main GUI component for all demo scenes.</summary>
    public class GUIMain : MonoBehaviour
    {

        #region Variables

        public Text Version;
        public Text Scene;
        public GameObject NoVoices;
        public Text Errors;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            Speaker.OnErrorInfo += errorInfoMethod;

            if (Version != null)
            {
                Version.text = Constants.ASSET_NAME + " - " + Constants.ASSET_VERSION;
            }

            if (Scene != null)
            {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                Scene.text = SceneManager.GetActiveScene().name;
#else
            Scene.text = Application.loadedLevelName;
#endif
            }

            if (Speaker.Voices.Count > 0 && NoVoices != null)
            {
                NoVoices.SetActive(false);
            }

            if (Errors != null)
            {
                Errors.text = string.Empty;
            }
        }

        void Update()
        {
            Cursor.visible = true;
        }

        void OnDestroy()
        {
            Speaker.OnErrorInfo -= errorInfoMethod;
        }
        #endregion

        #region Public methods

        public void OpenAssetURL()
        {
            Application.OpenURL(Constants.ASSET_URL);
        }

        public void OpenCTURL()
        {
            Application.OpenURL(Constants.ASSET_AUTHOR_URL);
        }

        public void Silence()
        {
            Speaker.Silence();
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion


        #region Callbacks

        private void errorInfoMethod(string errorInfo)
        {
            if (Errors != null)
            {
                Errors.text = errorInfo;
            }
        }

        #endregion

    }
}
// Copyright 2015-2016 www.crosstales.com