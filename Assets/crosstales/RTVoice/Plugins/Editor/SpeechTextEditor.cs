using UnityEngine;
using UnityEditor;
using Crosstales.RTVoice.Tool;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Custom editor for the 'SpeechText'-class.</summary>
    [CustomEditor(typeof(SpeechText))]
    [CanEditMultipleObjects]
    public class SpeechTextEditor : Editor
    {

        #region Variables

        private SpeechText script;

        #endregion

        #region Editor methods

        void OnEnable()
        {
            script = (SpeechText)target;
        }

        void OnDisable()
        {
            if (Helper.isEditorMode)
            {
                Speaker.Silence();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorHelper.SeparatorUI();

            if (script.isActiveAndEnabled)
            {
                if (Speaker.isTTSAvailable)
                {
                    GUILayout.Label("Test-Drive", EditorStyles.boldLabel);

                    if (Helper.isEditorMode)
                    {
                        if (GUILayout.Button(new GUIContent("Speak", "Speaks the text with the selected voice and settings.")))
                        {
                            script.Speak();
                        }

                        if (GUILayout.Button(new GUIContent("Silence", "Silence the active speaker.")))
                        {
                            script.Silence();
                        }

                        EditorHelper.SeparatorUI();

                        GUILayout.Label("Editor", EditorStyles.boldLabel);

                        if (GUILayout.Button(new GUIContent("Refresh AssetDatabase", "Refresh the AssetDatabase from the Editor.")))
                        {
                            refreshAssetDatabase();
                        }
                    }
                    else
                    {
                        GUILayout.Label("Disabled in Play-mode!");
                    }
                }
                else
                {
                    EditorHelper.NoVoicesUI();
                }
            }
            else
            {
                GUILayout.Label("Script is disabled!", EditorStyles.boldLabel);
            }
        }

        #endregion

        #region Private methods

        private void refreshAssetDatabase()
        {
            if (Helper.isEditorMode)
            {
                //Debug.Log("Refresh AssetDatabase");
                AssetDatabase.Refresh();
            }
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com