using UnityEngine;
using UnityEditor;
using System.Threading;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Base class for editor windows.</summary>
    public abstract class ConfigBase : EditorWindow
    {

        #region Variables

        protected static string updateText = UpdateCheck.TEXT_NOT_CHECKED;

        private static Thread worker;

        #endregion

        #region Protected methods

        protected static void showConfiguration()
        {
            GUILayout.Label("Global Settings", EditorStyles.boldLabel);
            Constants.DEBUG = EditorGUILayout.Toggle(new GUIContent("Debug", "Enable or disable debug logs (default: off)."), Constants.DEBUG);

            Constants.UPDATE_CHECK = EditorGUILayout.BeginToggleGroup(new GUIContent("Update check", "Enable or disable the update-check (default: on)."), Constants.UPDATE_CHECK);
            EditorGUI.indentLevel++;
            Constants.UPDATE_OPEN_UAS = EditorGUILayout.Toggle(new GUIContent("Open UAS-site", "Automatically opens the direct link to 'Unity AssetStore' if an update was found (default: off)."), Constants.UPDATE_OPEN_UAS);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();

            Constants.PREFAB_PATH = EditorGUILayout.TextField(new GUIContent("Prefab path", "Path to the prefabs."), Constants.PREFAB_PATH);
            Constants.PREFAB_AUTOLOAD = EditorGUILayout.Toggle(new GUIContent("Prefab auto-load", "Enable or disable auto-loading of the prefabs to the scene (default: on)."), Constants.PREFAB_AUTOLOAD);

            Constants.AUDIOFILE_PATH = EditorGUILayout.TextField(new GUIContent("Audio path", "Path to the generated audio files."), Constants.AUDIOFILE_PATH);
            Constants.AUDIOFILE_AUTOMATIC_DELETE = EditorGUILayout.Toggle(new GUIContent("Audio auto-delete", "Enable or disable auto-delete of the generated audio files (default: on)."), Constants.AUDIOFILE_AUTOMATIC_DELETE);

            Constants.ENFORCE_32BIT_WINDOWS = EditorGUILayout.Toggle(new GUIContent("Enforce 32bit voices", "Enforce 32bit versions of voices under Windows. (default: off)."), Constants.ENFORCE_32BIT_WINDOWS);
        }

        protected static void showAbout()
        {
            GUILayout.Label(Constants.ASSET_NAME, EditorStyles.boldLabel);
            GUILayout.Label("Version:\t" + Constants.ASSET_VERSION);

            GUILayout.Space(6);
            GUILayout.Label("Web:\t" + Constants.ASSET_AUTHOR_URL);
            GUILayout.Label("Email:\t" + Constants.ASSET_CONTACT);

            GUILayout.Space(12);
            GUILayout.Label("© 2015-2016 by " + Constants.ASSET_AUTHOR);

            EditorHelper.SeparatorUI();

            if (worker == null || (worker != null && !worker.IsAlive))
            {
                if (GUILayout.Button(new GUIContent("Check for update", "Checks for available updates of " + Constants.ASSET_NAME)))
                {

                    worker = new Thread(() => UpdateCheck.UpdateCheckForEditor(out updateText));
                    worker.Start();
                }
            }
            else
            {
                GUILayout.Label("Checking... Please wait.", EditorStyles.boldLabel);
            }

            Color fgColor = GUI.color;

            if (updateText.Equals(UpdateCheck.TEXT_NOT_CHECKED))
            {
                GUI.color = Color.cyan;
                GUILayout.Label(updateText);
            }
            else if (updateText.Equals(UpdateCheck.TEXT_NO_UPDATE))
            {
                GUI.color = Color.green;
                GUILayout.Label(updateText);
            }
            else
            {
                GUI.color = Color.yellow;
                GUILayout.Label(updateText);

                if (GUILayout.Button(new GUIContent("Download", "Opens the 'Unity AssetStore' for downloading the latest version.")))
                {
                    Application.OpenURL(Constants.ASSET_URL);
                }
            }

            GUI.color = fgColor;

            EditorHelper.SeparatorUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Manual", "Opens the manual.")))
            {
                Application.OpenURL(Constants.ASSET_MANUAL_URL);
            }
            if (GUILayout.Button(new GUIContent("API", "Opens the API.")))
            {
                Application.OpenURL(Constants.ASSET_API_URL);
            }
            if (GUILayout.Button(new GUIContent("Forum", "Opens the forum page.")))
            {
                Application.OpenURL(Constants.ASSET_FORUM_URL);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Unity AssetStore", "Opens the 'Unity AssetStore' page.")))
            {
                Application.OpenURL(Constants.ASSET_URL);
            }

            if (GUILayout.Button(new GUIContent("Product", "Opens the product page.")))
            {
                Application.OpenURL(Constants.ASSET_CT_URL);
            }

            if (GUILayout.Button(new GUIContent(Constants.ASSET_AUTHOR, "Opens the 'crosstales' page.")))
            {
                Application.OpenURL(Constants.ASSET_AUTHOR_URL);
            }
            EditorGUILayout.EndHorizontal();
        }

        protected static void save()
        {
            EditorPrefs.SetBool(Constants.KEY_DEBUG, Constants.DEBUG);
            EditorPrefs.SetBool(Constants.KEY_UPDATE_CHECK, Constants.UPDATE_CHECK);
            EditorPrefs.SetBool(Constants.KEY_UPDATE_OPEN_UAS, Constants.UPDATE_OPEN_UAS);
            EditorPrefs.SetString(Constants.KEY_PREFAB_PATH, Constants.PREFAB_PATH);
            EditorPrefs.SetBool(Constants.KEY_PREFAB_AUTOLOAD, Constants.PREFAB_AUTOLOAD);
            EditorPrefs.SetString(Constants.KEY_AUDIOFILE_PATH, Constants.AUDIOFILE_PATH);
            EditorPrefs.SetBool(Constants.KEY_AUDIOFILE_AUTOMATIC_DELETE, Constants.AUDIOFILE_AUTOMATIC_DELETE);
            EditorPrefs.SetBool(Constants.KEY_ENFORCE_32BIT_WINDOWS, Constants.ENFORCE_32BIT_WINDOWS);

            if (Constants.DEBUG)
                Debug.Log("Config data saved");
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com