using System;
using System.Collections;
using System.Collections.Generic;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;
using System.Runtime.InteropServices;

namespace Crosstales.RTVoice.Provider
{

    /// <summary>iOS voice provider.</summary>
    public class VoiceProviderIOS : BaseVoiceProvider
    {
        #region Variables

        private const string extension = "none";

#if (UNITY_IOS)
        private static string[] speechTextArray;

        private static string State;

        private static int wordIndex = 0;

        private static bool getVoicesCalled = false;
        private static bool isWorking = false;

        private static WrapperNative wrapperNative;

        private static List<Voice> defaultVoices;
#endif

        #endregion


        public VoiceProviderIOS()
        {
#if (UNITY_IOS)
            defaultVoices = new List<Voice>();

            defaultVoices.Add(new Voice("Maged", "ar-SA", "ar-SA"));
            defaultVoices.Add(new Voice("Zuzana", "cs-CZ", "cs-CZ"));
            defaultVoices.Add(new Voice("Sara ", "da-DK", "da-DK"));
            defaultVoices.Add(new Voice("Anna ", "de-DE", "de-DE"));
            defaultVoices.Add(new Voice("Melina", "el-GR", "el-GR"));
            defaultVoices.Add(new Voice("Karen", "en-AU", "en-AU"));
            defaultVoices.Add(new Voice("Daniel", "en-GB", "en-GB"));
            defaultVoices.Add(new Voice("Moira", "en-IE", "en-IE"));
            defaultVoices.Add(new Voice("Samantha", "en-US", "en-US"));
            defaultVoices.Add(new Voice("Tessa", "en-ZA", "en-ZA"));
            defaultVoices.Add(new Voice("Monica", "es-ES", "es-ES"));
            defaultVoices.Add(new Voice("Paulina", "es-MX", "es-MX"));
            defaultVoices.Add(new Voice("Satu ", "fi-FI", "fi-FI"));
            defaultVoices.Add(new Voice("Amelie", "fr-CA", "fr-CA"));
            defaultVoices.Add(new Voice("Thomas", "fr-FR", "fr-FR"));
            defaultVoices.Add(new Voice("Lekha", "hi-IN", "hi-IN"));
            defaultVoices.Add(new Voice("Mariska", "hu-HU", "hu-HU"));
            defaultVoices.Add(new Voice("Damayanti", "id-ID", "id-ID"));
            defaultVoices.Add(new Voice("Alice", "it-IT", "it-IT"));
            defaultVoices.Add(new Voice("Kyoko", "ja-JP", "ja-JP"));
            defaultVoices.Add(new Voice("Yuna ", "ko-KR", "ko-KR"));
            defaultVoices.Add(new Voice("Ellen", "nl-BE", "nl-BE"));
            defaultVoices.Add(new Voice("Xander", "nl-NL", "nl-NL"));
            defaultVoices.Add(new Voice("Nora", "no-NO", "no-NO"));
            defaultVoices.Add(new Voice("Zosia", "pl-PL", "pl-PL"));
            defaultVoices.Add(new Voice("Luciana", "pt-BR", "pt-BR"));
            defaultVoices.Add(new Voice("Joana", "pt-PT", "pt-PT"));
            defaultVoices.Add(new Voice("Ioana", "ro-RO", "ro-RO"));
            defaultVoices.Add(new Voice("Milena", "ru-RU", "ru-RU"));
            defaultVoices.Add(new Voice("Laura", "sk-SK", "sk-SK"));
            defaultVoices.Add(new Voice("Alva", "sv-SE", "sv-SE"));
            defaultVoices.Add(new Voice("Kanya", "th-TH", "th-TH"));
            defaultVoices.Add(new Voice("Yelda", "tr-TR", "tr-TR"));
            defaultVoices.Add(new Voice("Ting-Ting", "zh-CN", "zh-CN"));
            defaultVoices.Add(new Voice("Sin-Ji", "zh-HK", "zh-HK"));
            defaultVoices.Add(new Voice("Mei-Jia", "zh-TW", "zh-TW"));
#endif
        }

        #region Bridge declaration and methods

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        [DllImport("__Internal")]
        extern static public void Stop();

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        [DllImport("__Internal")]
        extern static public void GetVoices();

        /// <summary>Bridge to the native tts system</summary>
        /// <param name="gameObject">Receiving gameobject for the messages from iOS</param>
        /// <param name="text">Text to speak.</param>
        /// <param name="rate">Speech rate of the speaker in percent (default: 0.5, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (default: 1, optional).</param>
        /// <param name="culture">Culture of the voice to speak (optional).</param>
        [DllImport("__Internal")]
        extern static public void Speak(string gameObject, string text, float rate = 0.5f, float pitch = 1f, float volume = 1f, string culture = "");

        /// <summary>Receives all voices</summary>
        /// <param name="voicesText">All voices as text string.</param>
        public static void SetVoices(string voicesText)
        {
#if (UNITY_IOS)
            string[] v = voicesText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (v.Length % 2 == 0)
            {
                cachedVoices = new List<Voice>();

                string name;
                string culture;
                Voice newVoice;

                for (int ii = 0; ii < v.Length; ii += 2)
                {
                    name = v[ii];
                    culture = v[ii + 1];
                    newVoice = new Voice(name, "iOS voice: " + name + " " + culture, culture);

                    if (Constants.DEBUG)
                        UnityEngine.Debug.Log("Voice added: " + newVoice);

                    cachedVoices.Add(newVoice);
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("Voice-string contains an uneven number of elements!");
            }
#endif
        }

        /// <summary>Receives the state of the speaker.</summary>
        /// <param name="state">The state of the speaker.</param>
        public static void SetState(string state)
        {
#if (UNITY_IOS)
            if (state.Equals("Start"))
            {
                // do nothing
            }
            else if (state.Equals("Finsish"))
            {
                isWorking = false;
            }
            else
            { //cancel
                isWorking = false;
            }
#endif
        }

        /// <summary>Called everytime a new word is spoken.</summary>
        public static void WordSpoken()
        {
#if (UNITY_IOS)
            if (wrapperNative != null)
            {
                onSpeakNativeCurrentWord(wrapperNative, speechTextArray, wordIndex);
                wordIndex++;
            }
#endif
        }

        #endregion

        #region Implemented methods

        public override string AudioFileExtension
        {
            get
            {
                return extension;
            }
        }


        public override List<Voice> Voices
        {
            get
            {
#if (UNITY_IOS)
                if (cachedVoices == null)
                {

                    if (!getVoicesCalled)
                    {
                        GetVoices();

                        getVoicesCalled = true;
                    }

                    return defaultVoices;

                }
#endif
                return cachedVoices;
            }
        }

        public override void Silence()
        {
            silence = true;

            Stop();
        }

        public override IEnumerator SpeakNative(WrapperNative wrapper)
        {

#if (UNITY_IOS)
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty!");
                    yield return null;
                }
                else
                {
                    yield return null;
                    string voiceName = string.Empty;
                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Culture))
                    {
                        UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Culture' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        voiceName = wrapper.Voice.Culture;
                    }
                    silence = false;

                    onSpeakNativeStart(wrapper);
                    isWorking = true;

                    speechTextArray = Helper.CleanText(wrapper.Text, false).Split(splitCharWords, StringSplitOptions.RemoveEmptyEntries);
                    wordIndex = 0;
                    wrapperNative = wrapper;
                    Speak(Constants.RTVOICE_SCENE_OBJECT_NAME, wrapper.Text, calculateRate(wrapper.Rate), wrapper.Pitch, wrapper.Volume, voiceName);

                    do
                    {
                        yield return null;
                    } while (isWorking && !silence);

                    if (Constants.DEBUG)
                        UnityEngine.Debug.Log("Text spoken: " + wrapper.Text);

                    wrapperNative = null;
                    onSpeakNativeComplete(wrapper);
                }
            }
#else
            yield return null;
#endif
        }


        public override IEnumerator Speak(Wrapper wrapper)
        {

#if (UNITY_IOS)
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty!");
                    yield return null;
                }
                else
                {
                    yield return null;
                    string voiceName = string.Empty;
                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Culture))
                    {
                        UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Culture' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        voiceName = wrapper.Voice.Culture;
                    }

                    onSpeakAudioGenerationStart(wrapper);

                    onSpeakStart(wrapper);

                    if (wrapper.SpeakImmediately)
                    {

                        silence = false;
                        isWorking = true;

                        //speechTextArray = Helper.CleanText(wrapper.Text, false).Split(splitCharWords, StringSplitOptions.RemoveEmptyEntries);
                        Speak(Constants.RTVOICE_SCENE_OBJECT_NAME, wrapper.Text, calculateRate(wrapper.Rate), wrapper.Pitch, wrapper.Volume, voiceName);

                        do
                        {
                            yield return null;
                        } while (isWorking && !silence);

                        if (Constants.DEBUG)
                            UnityEngine.Debug.Log("Text spoken: " + wrapper.Text);

                    }
                    else
                    {
                        string message = "SpeakImmediately can not be false on iOS!";
                        onErrorInfo(message);
                        UnityEngine.Debug.LogWarning(message);
                    }

                    onSpeakComplete(wrapper);

                    onSpeakAudioGenerationComplete(wrapper);
                }
            }
#else
            yield return null;
#endif
        }

        #endregion

        #region Private methods

        private float calculateRate(float rate)
        {
            if (rate <= 1)
            {
                rate = rate / 2;
            }
            else if (rate > 1)
            {
                rate = (rate + 1) * 0.25f;
            }
            else
            {
                rate = 0.5f;
            }
            return rate;
        }


        #endregion

        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Wrapper wrapper)
        {
            UnityEngine.Debug.LogError("GenerateInEditor is not supported for Unity iOS!");
        }

        public override void SpeakNativeInEditor(WrapperNative wrapper)
        {
            UnityEngine.Debug.LogError("SpeakNativeInEditor is not supported for Unity iOS!");
        }
#endif

        #endregion
    }
}