using UnityEngine;
using System;
using System.Text;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Model
{
    /// <summary>Wrapper for "Speak"-function calls.</summary>
    public class Wrapper
    {
        #region Variables

        /// <summary>UID of the speech.</summary>
        public Guid Uid;

        /// <summary>Text for the speech.</summary>
        public string Text;

        /// <summary>AudioSource for the speech.</summary>
        public AudioSource Source;

        /// <summary>Voice for the speech.</summary>
        public Voice Voice;

        /// <summary>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</summary>
        public bool SpeakImmediately;

        /// <summary>Output file (without extension) for the generated audio.</summary>
        public string OutputFile;

        private float rate;
        private float pitch;
        private float volume;

        #endregion

        #region Properties

        /// <summary>Rate of the speech (values: 0-3).</summary>
        public float Rate
        {
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
        /// <param name="source">AudioSource for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="speakImmediately">>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        /// <param name="outputFile">Output file (without extension) for the generated audio.</param>
        public Wrapper(string text, AudioSource source = null, Voice voice = null, bool speakImmediately = true, float rate = 1f, float pitch = 1f, float volume = 1f, string outputFile = "")
        {
            Uid = Guid.NewGuid();
            Text = text;
            Source = source;
            Voice = voice;
            SpeakImmediately = speakImmediately;
            Rate = rate;
            Pitch = pitch;
            Volume = volume;
            OutputFile = outputFile;
        }

        /// <summary>Instantiate the class.</summary>
        /// <param name="uid">UID of the speech.</param>
        /// <param name="text">Text for the speech.</param>
        /// <param name="source">AudioSource for the speech.</param>
        /// <param name="voice">Voice for the speech.</param>
        /// <param name="speakImmediately">>Speak immediatlely after the audio generation. Only works if 'Source' is not null.</param>
        /// <param name="rate">Rate of the speech (values: 0-3).</param>
        /// <param name="pitch">Pitch of the speech (values: 0-2).</param>
        /// <param name="volume">Volume of the speech (values: 0-1, Windows only).</param>
        /// <param name="outputFile">Output file (without extension) for the generated audio.</param>
        public Wrapper(Guid uid, string text, AudioSource source = null, Voice voice = null, bool speakImmediately = true, float rate = 1f, float pitch = 1f, float volume = 1f, string outputFile = "") : this(text, source, voice, speakImmediately, rate, pitch, volume, outputFile)
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

            result.Append("Source='");
            result.Append(Source);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Voice='");
            result.Append(Voice);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("SpeakImmediately='");
            result.Append(SpeakImmediately);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Rate='");
            result.Append(Rate);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Pitch='");
            result.Append(Pitch);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("Volume='");
            result.Append(Volume);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER);

            result.Append("OutputFile='");
            result.Append(OutputFile);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com