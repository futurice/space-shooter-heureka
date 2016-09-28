using UnityEngine;
using UnityEditor;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Editor helper class.</summary>
    public static class EditorHelper
    {
        /// <summary>Start index inside the "Tools"-menu.</summary>
        public const int MENU_ID = 2000;

        /// <summary>Shows the "no voices found"-UI.</summary>
        public static void NoVoicesUI()
        {
            Color guiColor = GUI.color;

            GUILayout.Label("Could Not Load Voices!", EditorStyles.boldLabel);

            if (isRTVoiceInScene)
            {
                GUI.color = Color.red;
                GUILayout.Label("TTS on your machine is not possible.");
            }
            else
            {
                GUI.color = Color.yellow;
                GUILayout.Label("Did you add the 'RTVoice'-prefab to the scene?");

                GUILayout.Space(8);

                if (GUILayout.Button(new GUIContent("Add RTVoice", "Add the RTVoice-prefab to the current scene.")))
                {
                    AddRTVoice();
                }
            }

            GUI.color = guiColor;
        }

        /// <summary>Shows a separator-UI.</summary>
        public static void SeparatorUI(int space = 20)
        {
            GUILayout.Space(space);
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        /// <summary>Adds the 'RTVoice'-prefab to the scene.</summary>
        public static void AddRTVoice()
        {
            if (!isRTVoiceInScene)
            {
                PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(Constants.PREFAB_PATH + "RTVoice.prefab", typeof(GameObject)));
            }
        }

        /// <summary>Checks if the 'RTVoice'-prefab is in the scene.</summary>
        /// <returns>True if the 'RTVoice'-prefab is in the scene.</returns>
        public static bool isRTVoiceInScene
        {
            get
            {
                return GameObject.Find(Constants.RTVOICE_SCENE_OBJECT_NAME) != null;
            }
        }
    }
}
// Copyright 2016 www.crosstales.com