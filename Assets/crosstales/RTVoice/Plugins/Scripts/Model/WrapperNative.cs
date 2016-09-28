using UnityEngine;
using System;
using System.Text;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Model
{
    /// <summary>Wrapper (native mode) for "SpeakNative"-function calls.</summary>
    [Serializable]
    public class WrapperNative
    {
        #region Variables

        /// <summary>UID of the speech.</summary>
        public Guid Uid;

        /// <summary>Text for the speech.</summary>
        public string Text;

        /// <summary>Voice for the speech.</summary>
        public Voice Voice;

        private float rate;
        private float pitch;
        private float volume;
        #endregion

        #region Properties

        /// <summary>Rate of the speech (values: 0-3).</summary>
        public float Rate {
            get
            {
                return rate;
            }

            set
            {
                rate = Mathf.Clamp(value, 0f, 3f);
            }
        }

        /// <summary>Pitch of the speech (values: 0-2).</summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                pitch = Mathf.Clamp(value, 0f, 2f);
            }
        }

        /// <summary>Volume of the speech (values: 0-1).</summary>
        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        #endregion

        #region Constructors

        /// <summary>Instantiate the class.</summary>
        /// <param name="text">Text for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        public WrapperNative(string text, Voice voice = null, float rate = 1f, float pitch = 1f, float volume = 1f)
        {
            Uid = Guid.NewGuid();
            Text = text;
            Voice = voice;
            Rate = rate;
            Pitch = pitch;
            Volume = volume;
        }

        /// <summary>Instantiate the class.</summary>
        /// <param name="uid">UID of the speech.</param>
        /// <param name="text">Text for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        public WrapperNative(Guid uid, string text, Voice voice = null, float rate = 1f, float pitch = 1f, float volume = 1f) : this(text, voice, rate, pitch, volume)
        {
            Uid = uid;
        }

        #endregion

        #region Overridden methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(GetType().Name);
            result.Append(Constants.TEXT_TOSTRING_START);


            result.Append("Uid='");
            result.Append(Uid);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Text='");
            result.Append(Text);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Voice='");
            result.Append(Voice);
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
// Copyright 2015-2016 www.crosstales.com