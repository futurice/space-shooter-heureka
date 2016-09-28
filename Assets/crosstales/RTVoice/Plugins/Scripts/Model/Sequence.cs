using UnityEngine;
using System;
using System.Text;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Model
{
    /// <summary>Model for a sequence.</summary>
    [Serializable]
    public class Sequence
    {

        #region Variables

        /// <summary>Text to speak.</summary>
        [Tooltip("Text to speak.")]
        [Multiline]
        public string Text;

        /// <summary>Name of the RT-Voice under Windows (optional).</summary>
        [Tooltip("Name of the RT-Voice under Windows (optional).")]
        public string RTVoiceNameWindows = string.Empty;

        /// <summary>Name of the RT-Voice under macOS (optional).</summary>
        [Tooltip("Name of the RT-Voice under macOS (optional).")]
        public string RTVoiceNameMac = string.Empty;

        /// <summary>Name of the RT-Voice under Android.</summary>
        [Tooltip("Name of the RT-Voice under Android.")]
        public string RTVoiceNameAndroid = string.Empty;

        /// <summary>Name of the RT-Voice under iOS.</summary>
        [Tooltip("Name of the RT-Voice under iOS.")]
        public string RTVoiceNameIOS = string.Empty;

        /// <summary>Speak mode (default = 'Speak').</summary>
        [Tooltip("Speak mode (default = 'Speak').")]
        public SpeakMode Mode = SpeakMode.Speak;

        [Header("Optional Settings")]
        /// <summary>AudioSource for the output (optional).</summary>
        [Tooltip("AudioSource for the output (optional).")]
        public AudioSource Source;

        /// <summarySpeech rate of the speaker in percent (1 = 100%, default: 1, optional).</summary>
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

        [HideInInspector]
        public bool initalized = false;

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

        #region Overridden methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(GetType().Name);
            result.Append(Constants.TEXT_TOSTRING_START);

            result.Append("Text='");
            result.Append(Text);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("RTVoiceNameWindows='");
            result.Append(RTVoiceNameWindows);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("RTVoiceNameMac='");
            result.Append(RTVoiceNameMac);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("RTVoiceNameAndroid='");
            result.Append(RTVoiceNameAndroid);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("RTVoiceNameIOS='");
            result.Append(RTVoiceNameIOS);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Source='");
            result.Append(Source);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Rate='");
            result.Append(Rate);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Pitch='");
            result.Append(Pitch);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Volume='");
            result.Append(Volume);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }
        #endregion
    }
}
// Copyright 2016 www.crosstales.com