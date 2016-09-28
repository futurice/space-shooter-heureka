using UnityEngine;
using System;
using System.IO;

namespace Crosstales.RTVoice.Util
{
    /// <summary>Collected constants of very general utility for the asset.</summary>
    public static class Constants
    {

        #region Constant variables

        /// <summary>Name of the asset.</summary>
        public const string ASSET_NAME = "RTVoice PRO"; //PRO
        //public const string ASSET_NAME = "RTVoice"; //DLL

        /// <summary>Version of the asset.</summary>
        public const string ASSET_VERSION = "2.5.1";

        /// <summary>Build number of the asset.</summary>
        public const int ASSET_BUILD = 251;

        /// <summary>Create date of the asset (YYYY, MM, DD).</summary>
        public static readonly DateTime ASSET_CREATED = new DateTime(2015, 4, 29);

        /// <summary>Change date of the asset (YYYY, MM, DD).</summary>
        public static readonly DateTime ASSET_CHANGED = new DateTime(2016, 9, 20);

        /// <summary>Author of the asset.</summary>
        public const string ASSET_AUTHOR = "crosstales LLC";

        /// <summary>URL of the asset author.</summary>
        public const string ASSET_AUTHOR_URL = "http://www.crosstales.com";

        /// <summary>URL of the asset.</summary>
        public const string ASSET_URL = "https://www.assetstore.unity3d.com/#!/content/41068"; //PRO
        //public const string ASSET_URL = "https://www.assetstore.unity3d.com/#!/content/48394"; //DLL

        /// <summary>URL for update-checks of the asset</summary>
        public const string ASSET_UPDATE_CHECK_URL = "http://www.crosstales.com/media/assets/rtvoice_versions.txt";

        /// <summary>Contact to the owner of the asset.</summary>
        public const string ASSET_CONTACT = "rtvoice@crosstales.com";

        /// <summary>UID of the asset.</summary>
        public static readonly Guid ASSET_UID = new Guid("181f4dab-261f-4746-85f8-849c2866d353"); //PRO
        //public static readonly Guid ASSET_UID = new Guid("1ffe8fd4-4e17-497b-9df7-7cd9200d0941"); //DLL

        /// <summary>URL of the asset manual.</summary>
        public const string ASSET_MANUAL_URL = "http://www.crosstales.com/en/assets/rtvoice/RTVoice-doc.pdf";

        /// <summary>URL of the asset API.</summary>
        public const string ASSET_API_URL = "http://goo.gl/6w4Fy0"; // http://www.crosstales.com/en/assets/rtvoice/api/
        //public const string ASSET_API_URL = "http://www.crosstales.com/en/assets/rtvoice/api/";

        /// <summary>URL of the asset forum.</summary>
        public const string ASSET_FORUM_URL = "http://goo.gl/Z6MZMl"; //ok 23.07.2016
        //public const string ASSET_FORUM_URL = "http://forum.unity3d.com/threads/rt-voice-run-time-text-to-speech-solution.340046/";

        /// <summary>URL of the asset in crosstales.</summary>
        public const string ASSET_CT_URL = "http://www.crosstales.com/en/assets/rtvoice/";

        /// <summary>Name of the RT-Voice scene object.</summary>
        public const string RTVOICE_SCENE_OBJECT_NAME = "RTVoice";

        // Keys for the configuration of the asset
        private const string KEY_PREFIX = "RTVOICE_CFG_";
        public const string KEY_DEBUG = KEY_PREFIX + "DEBUG";
        public const string KEY_UPDATE_CHECK = KEY_PREFIX + "UPDATE_CHECK";
        public const string KEY_UPDATE_OPEN_UAS = KEY_PREFIX + "UPDATE_OPEN_UAS";
        public const string KEY_PREFAB_PATH = KEY_PREFIX + "PREFAB_PATH";
        public const string KEY_PREFAB_AUTOLOAD = KEY_PREFIX + "PREFAB_AUTOLOAD";
        public const string KEY_AUDIOFILE_PATH = KEY_PREFIX + "AUDIOFILE_PATH";
        public const string KEY_AUDIOFILE_AUTOMATIC_DELETE = KEY_PREFIX + "AUDIOFILE_AUTOMATIC_DELETE";
        public const string KEY_UPDATE_DATE = KEY_PREFIX + "UPDATE_DATE";
        public const string KEY_ENFORCE_32BIT_WINDOWS = KEY_PREFIX + "ENFORCE_32BIT_WINDOWS";
        #endregion

        #region Changable variables
        /// <summary>Enable or disable debug logging for the asset.</summary>
        public static bool DEBUG = false;

        /// <summaryEnable or disable update-checks for the asset.</summary>
        public static bool UPDATE_CHECK = true;

        /// <summaryOpen the UAS-site when an update is found.</summary>
        public static bool UPDATE_OPEN_UAS = false;

        /// <summary>Path of the prefabs.</summary>
        public static string PREFAB_PATH = "Assets/crosstales/RTVoice/Prefabs/";

        /// <summary>Automatically load and add the prefabs to the scene.</summary>
        public static bool PREFAB_AUTOLOAD = true;

        /// <summary>Path to the generated audio files.</summary>
        public static string AUDIOFILE_PATH = Path.GetTempPath(); //path to the generated audio files

        /// <summary>Automatically delete the generated audio files.</summary>
        public static bool AUDIOFILE_AUTOMATIC_DELETE = true;

        /// <summary>Enforce 32bit versions of voices under Windows.</summary>
        public static bool ENFORCE_32BIT_WINDOWS = false;

        // Technical settings

        /// <summary>Location of the TTS-wrapper under Windows (Editor).</summary>
        public static string TTS_WINDOWS_EDITOR = Application.dataPath + @"/crosstales/RTVoice/Plugins/Windows/RTVoiceTTSWrapper.exe";

        /// <summary>Location of the TTS-wrapper under Windows (Editor).</summary>
        public static string TTS_WINDOWS_EDITOR_x86 = Application.dataPath + @"/crosstales/RTVoice/Plugins/Windows/RTVoiceTTSWrapper_x86.exe";

        /// <summary>Location of the TTS-wrapper under Windows (stand-alone).</summary>
        public static string TTS_WINDOWS_BUILD = Application.dataPath + @"/RTVoiceTTSWrapper.exe";

        /// <summary>Location of the TTS-system under MacOS.</summary>
        public static string TTS_MACOS = "say";

        /// <summary>Kill processes after 5000 milliseconds.</summary>
        public static int TTS_KILL_TIME = 5000;


        // Text fragments for the asset
        public static string TEXT_TOSTRING_START = " {";
        public static string TEXT_TOSTRING_END = "}";
        public static string TEXT_TOSTRING_DELIMITER = "', ";
        public static string TEXT_TOSTRING_DELIMITER_END = "'";

        #endregion

    }
}
// Copyright 2015-2016 www.crosstales.com