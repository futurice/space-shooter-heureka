using UnityEngine;
using System;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Provider;

namespace Crosstales.RTVoice
{
    /// <summary>Wrapper of the main component from RTVoice for MonoBehaviour-access (like "SendMessage").</summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [HelpURL("http://www.crosstales.com/en/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_live_speaker.html")]
    public class LiveSpeaker : MonoBehaviour
    {

        private static char[] splitChar = new char[] { ';' };

        #region Public methods

        /// <summary>Speaks a text with a given wrapper -> native mode.</summary>
        /// <param name="wrapper">Wrapper with the speech details.</param>
        public void SpeakNative(WrapperNative wrapper)
        {
            Speaker.SpeakNative(wrapper);
        }

        /// <summary>Speaks a text with a given array of arguments (native mode).</summary>
        /// <param name="args">Argument string delimited by ';': 0 = text, 1 = culture (optional), 2 = voiceName (optional), 3 = rate (optional), 4 = volume (optional), 5 = pitch (optional).</param>
        public void SpeakNative(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                SpeakNative(args.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                Debug.LogWarning("'args' is null or empty!");
            }
        }

        /// <summary>Speaks a text with a given array of arguments (native mode).</summary>
        /// <param name="args">Argument index: 0 = text, 1 = culture (optional), 2 = voiceName (optional), 3 = rate (optional), 4 = volume (optional), 5 = pitch (optional).</param>
        public void SpeakNative(string[] args)
        {
            if (args != null && args.Length >= 1)
            {

                string text = args[0];

                string culture = null;
                if (args.Length >= 2)
                {
                    culture = args[1];
                }

                Voice voice = null;
                if (args.Length >= 3)
                {
                    voice = Speaker.VoiceForName(args[2]);
                }

                float rate = 1f;
                if (args.Length >= 4)
                {
                    if (!float.TryParse(args[3], out rate))
                    {
                        Debug.LogWarning("Argument 3 (= rate) is not a number: '" + args[3] + "'");
                        rate = 1f;
                    }
                }

                float volume = 1f;
                if (args.Length >= 5)
                {
                    if (!float.TryParse(args[4], out volume))
                    {
                        Debug.LogWarning("Argument 4 (= volume) is not a number: '" + args[4] + "'");
                        volume = 1f;
                    }
                }

                float pitch = 1f;
                if (args.Length >= 6)
                {
                    if (!float.TryParse(args[5], out pitch))
                    {
                        Debug.LogWarning("Argument 5 (= pitch) is not a number: '" + args[5] + "'");
                        pitch = 1f;
                    }
                }

                if (voice == null)
                {
                    voice = Speaker.VoiceForCulture(culture);
                }

                SpeakNative(new WrapperNative(text, voice, rate, pitch, volume));
            }
            else
            {
                Debug.LogError("'args' is null or wrong number of arguments given!" + Environment.NewLine + "Please verify that you pass a string-array with at least one argument (text).");
            }
        }

        /// <summary>Speaks a text with a given wrapper.</summary>
        /// <param name="wrapper">Wrapper with the speech details.</param>
        public void Speak(Wrapper wrapper)
        {
            Speaker.Speak(wrapper);
        }

        /// <summary>
        /// Speaks a text with a given array of arguments.
        /// <remarks>Important: you can't specify the AudioSource with this method!</remarks>
        /// </summary>
        /// <param name="args">Argument string delimited by ';': 0 = text, 1 = culture (optional), 2 = voiceName (optional), 3 = rate (optional), 4 = volume (optional), 5 = pitch (optional).</param>
        public void Speak(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                Speak(args.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                Debug.LogWarning("'args' is null or empty!");
            }
        }

        /// <summary>
        /// Speaks a text with a given array of arguments.
        /// <remarks>Important: you can't specify the AudioSource with this method!</remarks>
        /// </summary>
        /// <param name="args">Argument index: 0 = text, 1 = culture (optional), 2 = voiceName (optional), 3 = rate (optional), 4 = volume (optional), 5 = pitch (optional).</param>
        public void Speak(string[] args)
        {
            if (args != null && args.Length >= 1)
            {

                string text = args[0];

                string culture = null;
                if (args.Length >= 2)
                {
                    culture = args[1];
                }

                Voice voice = null;
                if (args.Length >= 3)
                {
                    voice = Speaker.VoiceForName(args[2]);
                }

                float rate = 1f;
                if (args.Length >= 4)
                {
                    if (!float.TryParse(args[3], out rate))
                    {
                        Debug.LogWarning("Argument 3 (= rate) is not a number: '" + args[3] + "'");
                        rate = 1f;
                    }
                }

                float volume = 1f;
                if (args.Length >= 5)
                {
                    if (!float.TryParse(args[4], out volume))
                    {
                        Debug.LogWarning("Argument 4 (= volume) is not a number: '" + args[4] + "'");
                        volume = 1f;
                    }
                }

                float pitch = 1f;
                if (args.Length >= 6)
                {
                    if (!float.TryParse(args[5], out pitch))
                    {
                        Debug.LogWarning("Argument 5 (= pitch) is not a number: '" + args[5] + "'");
                        pitch = 1f;
                    }
                }

                if (voice == null)
                {
                    voice = Speaker.VoiceForCulture(culture);
                }

                Speak(new Wrapper(text, null, voice, true, rate, pitch, volume));
            }
            else
            {
                Debug.LogError("'args' is null or wrong number of arguments given!" + Environment.NewLine + "Please verify that you pass a string-array with at least one argument (text).");
            }
        }

        /// <summary>Silence all active TTS-voices.</summary>
        public void Silence()
        {
            Speaker.Silence();
        }

        /// <summary>Sets all voices from iOS.</summary>
        /// <param name="voices">All voices from iOS.</param
        void SetVoices(string voices)
        {
            VoiceProviderIOS.SetVoices(voices);
        }

        /// <summary>The current spoken word from iOS.</summary>
        /// <param name="voices">Current spoken word from iOS.</param
        void WordSpoken(string word)
        {
            VoiceProviderIOS.WordSpoken();
        }

        /// <summary>Sets the state from iOS.</summary>
        /// <param name="voices">State from iOS.</param
        void SetState(string state)
        {
            VoiceProviderIOS.SetState(state);
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com