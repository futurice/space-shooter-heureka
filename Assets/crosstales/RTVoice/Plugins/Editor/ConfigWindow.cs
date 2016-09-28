using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Editor window extension.</summary>
    [InitializeOnLoad]
    public class ConfigWindow : ConfigBase
    {

        #region Variables

        private int tab = 0;
        private int lastTab = 0;
        private string text = "Test all your voices with different texts and settings.";
        private List<string> voices = new List<string>(50);
        private int voiceIndex;
        private float rate = 1f;
        private float volume = 1f;
        private bool silenced = true;

        public delegate void StopPlayback();
        public static event StopPlayback OnStopPlayback;

        #endregion

        #region Static constructor

        static ConfigWindow()
        {
            EditorApplication.update += onEditorUpdate;
        }

        #endregion

        #region EditorWindow methods

        [MenuItem("Tools/RTVoice/Configuration...", false, EditorHelper.MENU_ID + 1)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ConfigWindow));
        }

        void OnEnable()
        {
            titleContent = new GUIContent(Constants.ASSET_NAME);

            OnStopPlayback += silence;
        }

        void OnDisable()
        {
            //silence();
            Speaker.Silence();

            OnStopPlayback -= silence;
        }

        void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Configuration", "Test-Drive", "About" });

            if (tab != lastTab)
            {
                lastTab = tab;
                GUI.FocusControl(null);
            }

            if (tab == 0)
            {
                showConfiguration();

                EditorHelper.SeparatorUI();

                if (GUILayout.Button(new GUIContent("Save configuration", "Saves the configuration settings for this project.")))
                {
                    save();
                }
            }
            else if (tab == 1)
            {
                showTestDrive();
            }
            else
            {
                showAbout();
            }
        }

        #endregion

        #region Private methods

        private static void onEditorUpdate()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlaying || BuildPipeline.isBuildingPlayer)
            {
                onStopPlayback();
            }
        }

        private static void onStopPlayback()
        {
            if (OnStopPlayback != null)
            {
                OnStopPlayback();
            }
        }

        private void silence()
        {
            if (!silenced)
            {
                Speaker.Silence();
                silenced = true;
            }
        }

        private void showTestDrive()
        {
            if (Helper.isEditorMode)
            {

                voices.Clear();
                for (int voiceNumber = 0; voiceNumber < Speaker.Voices.Count; voiceNumber++)
                {
                    voices.Add(voiceNumber + ": " + Speaker.Voices[voiceNumber].Name);
                }

                if (voices.Count > 0)
                {
                    GUILayout.Label("Setup", EditorStyles.boldLabel);

                    text = EditorGUILayout.TextField("Text: ", text);

                    voiceIndex = EditorGUILayout.Popup("Voice", voiceIndex, voices.ToArray());
                    rate = EditorGUILayout.Slider("Rate", rate, 0f, 3f);

                    if (Helper.isWindowsPlatform)
                    {
                        volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
                    }

                    EditorHelper.SeparatorUI();

                    if (GUILayout.Button(new GUIContent("Speak", "Speaks the text with the selected voice and settings.")))
                    {
                        Speaker.SpeakNativeInEditor(text, Speaker.Voices[voiceIndex], rate, volume);
                        silenced = false;
                    }

                    if (GUILayout.Button(new GUIContent("Silence", "Silence all active speakers.")))
                    {
                        silence();
                    }
                }
                else
                {
                    EditorHelper.NoVoicesUI();
                }
            }
            else
            {
                GUILayout.Label("Disabled in Play-mode!");
            }
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com