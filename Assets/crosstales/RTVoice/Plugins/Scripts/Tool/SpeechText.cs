using UnityEngine;
using System.IO;
using System;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Tool
{
    /// <summary>Allows to speak and store generated audio.</summary>
    [HelpURL("http://www.crosstales.com/en/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_tool_1_1_speech_text.html")]
    public class SpeechText : MonoBehaviour
    {

        #region Variables

        /// <summary>Text to speak.</summary>
        [Tooltip("Text to speak.")]
        [Multiline]
        public string Text = "Hello world!";

        /// <summary>Name of the RT-Voice under Windows (optional).</summary>
        [Tooltip("Name of the RT-Voice under Windows (optional).")]
        public string RTVoiceNameWindows = "Microsoft David Desktop";

        /// <summary>Name of the RT-Voice under macOS (optional).</summary>
        [Tooltip("Name of the RT-Voice under macOS (optional).")]
        public string RTVoiceNameMac = "Alex";

        /// <summary>Name of the RT-Voice under Android.</summary>
        [Tooltip("Name of the RT-Voice under Android.")]
        public string RTVoiceNameAndroid = string.Empty;

        /// <summary>Name of the RT-Voice under iOS.</summary>
        [Tooltip("Name of the RT-Voice under iOS.")]
        public string RTVoiceNameIOS = "Daniel";

        /// <summary>Speak mode (default = Speak).</summary>
        [Tooltip("Speak mode (default = Speak).")]
        public SpeakMode Mode = SpeakMode.Speak;

        [Header("Optional Settings")]
        /// <summary>Fallback culture for the text (e.g. 'en', optional).</summary>
        [Tooltip("Fallback culture for the text (e.g. 'en', optional).")]
        public string Culture = "en";

        /// <summary>AudioSource for the output (optional).</summary>
        [Tooltip("AudioSource for the output (optional).")]
        public AudioSource Source;

        /// <summary>Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</summary>
        [Tooltip("Speech rate of the speaker in percent (1 = 100%, default: 1, optional).")]
        [Range(0f, 3f)]
        public float Rate = 1f;

        /// <summary>Speech pitch of the speaker in percent (1 = 100%, default: 1, optional, mobile only).</summary>
        [Tooltip("Speech pitch of the speaker in percent (1 = 100%, default: 1, optional, mobile only).")]
        [Range(0f, 2f)]
        public float Pitch = 1f;

        /// <summary>Volume of the speaker in percent (1 = 100%, default: 1, optional, Windows only).</summary>
        [Tooltip("Volume of the speaker in percent (1 = 100%, default: 1, optional, Windows only).")]
        [Range(0f, 1f)]
        public float Volume = 1f;

        [Header("Behaviour Settings")]
        /// <summary>Speak this text on start on/off (default: off).</summary>
        [Tooltip("Speak this text on start on/off (default: off).")]
        public bool PlayOnStart = false;

        [Header("Output File Settings")]
        /// <summary>Generate audio file on/off (default: off).</summary>
        [Tooltip("Generate audio file on/off (default: off).")]
        public bool GenerateAudioFile = false;

        /// <summary>File path for the generated audio.</summary>
        [Tooltip("File path for the generated audio.")]
        public string FilePath = @"_generatedAudio/";

        /// <summary>File name of the generated audio.</summary>
        [Tooltip("File name of the generated audio.")]
        public string FileName = "RTVGeneratedAudio";

        /// <summary>Is the generated file path inside the Assets-folder (current project)? If this option is enabled, it prefixes the path with 'Application.dataPath'.</summary>
        [Tooltip("Is the generated file path inside the Assets-folder (current project)? If this option is enabled, it prefixes the path with 'Application.dataPath'.")]
        public bool FileInsideAssets = true;

        private Guid uid = Guid.NewGuid(); //initalize it with a fake uid

        #endregion

        #region Properties

        /// <summary>Name of the RT-Voice.</summary>
        public string RTVoiceName
        {
            get
            {
                string result = null;

                if (Helper.isWindowsPlatform)
                {
                    result = RTVoiceNameWindows;
                }
                else if (Helper.isMacOSPlatform)
                {
                    result = RTVoiceNameMac;
                }
                else if (Helper.isAndroidPlatform)
                {
                    result = RTVoiceNameAndroid;
                }
                else
                {
                    result = RTVoiceNameIOS;
                }

                return result;
            }
        }

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            if (PlayOnStart)
            {
                Speak();
            }
        }

        #endregion

        #region Private methods

        public void Speak()
        {
            Silence();

            Voice voice = Speaker.VoiceForName(RTVoiceName);

            if (voice == null)
            {
                voice = Speaker.VoiceForCulture(Culture);
            }

            string path = null;

            if (GenerateAudioFile && !string.IsNullOrEmpty(FilePath))
            {
                if (FileInsideAssets)
                {
                    path = Helper.ValidatePath(Application.dataPath + @"/" + FilePath);
                }
                else
                {
                    path = Helper.ValidatePath(FilePath);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += FileName + Speaker.AudioFileExtension;
            }

            if (Helper.isEditorMode)
            {
#if UNITY_EDITOR
                Speaker.SpeakNativeInEditor(Text, voice, Rate, Volume, Pitch);
                if (GenerateAudioFile)
                {
                    Speaker.GenerateInEditor(Text, voice, Rate, Volume, path, Pitch);
                }
#endif
            }
            else
            {
                if (Mode == SpeakMode.Speak)
                {
                    uid = Speaker.Speak(Text, Source, voice, true, Rate, Volume, path, Pitch);
                }
                else
                {
                    uid = Speaker.SpeakNative(Text, voice, Rate, Volume, Pitch);
                }
            }
        }

        public void Silence()
        {
            if (Helper.isEditorMode)
            {
                Speaker.Silence();
            }
            else
            {
                Speaker.Silence(uid);
            }
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com