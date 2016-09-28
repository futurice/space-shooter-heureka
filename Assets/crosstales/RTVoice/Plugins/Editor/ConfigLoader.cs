using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.Collections;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt {
   /// <summary>Loads the configuration of the asset.</summary>
   [InitializeOnLoad]
   public static class ConfigLoader {

#region Constructor

      static ConfigLoader() {

         if (EditorPrefs.HasKey(Constants.KEY_DEBUG)) {
            Constants.DEBUG = EditorPrefs.GetBool(Constants.KEY_DEBUG);
         }

         if (EditorPrefs.HasKey(Constants.KEY_UPDATE_CHECK)) {
            Constants.UPDATE_CHECK = EditorPrefs.GetBool(Constants.KEY_UPDATE_CHECK);
         }

         if (EditorPrefs.HasKey(Constants.KEY_UPDATE_OPEN_UAS)) {
            Constants.UPDATE_OPEN_UAS = EditorPrefs.GetBool(Constants.KEY_UPDATE_OPEN_UAS);
         }

         if (EditorPrefs.HasKey(Constants.KEY_PREFAB_PATH)) {
            Constants.PREFAB_PATH = EditorPrefs.GetString(Constants.KEY_PREFAB_PATH);
         }

         if (EditorPrefs.HasKey(Constants.KEY_PREFAB_AUTOLOAD)) {
            Constants.PREFAB_AUTOLOAD = EditorPrefs.GetBool(Constants.KEY_PREFAB_AUTOLOAD);
         }

         if (EditorPrefs.HasKey(Constants.KEY_AUDIOFILE_PATH)) {
            Constants.AUDIOFILE_PATH = EditorPrefs.GetString(Constants.KEY_AUDIOFILE_PATH);
         }

         if (EditorPrefs.HasKey(Constants.KEY_AUDIOFILE_AUTOMATIC_DELETE)) {
            Constants.AUDIOFILE_AUTOMATIC_DELETE = EditorPrefs.GetBool(Constants.KEY_AUDIOFILE_AUTOMATIC_DELETE);
         }

            if (EditorPrefs.HasKey(Constants.KEY_ENFORCE_32BIT_WINDOWS))
            {
                Constants.ENFORCE_32BIT_WINDOWS = EditorPrefs.GetBool(Constants.KEY_ENFORCE_32BIT_WINDOWS);
            }

            //Constants.TTS_WINDOWS_EDITOR = Application.dataPath + @"/<NEW PATH>/RTVoice/Plugins/RTVoiceTTSWrapper/RTVoiceTTSWrapper.exe";

            if (Constants.DEBUG)
            Debug.Log("Config data loaded");
      }

#endregion
   }
}
// Copyright 2016 www.crosstales.com