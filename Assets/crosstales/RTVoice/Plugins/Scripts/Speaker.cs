using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Model.Event;
using Crosstales.RTVoice.Provider;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice
{
    /// <summary>Main component of RTVoice.</summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [HelpURL("http://www.crosstales.com/en/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_speaker.html")]
    public class Speaker : MonoBehaviour
    {

        #region Variables

        private Dictionary<Guid, AudioSource> removeSources = new Dictionary<Guid, AudioSource>();

        private static BaseVoiceProvider voiceProvider;
        private static Speaker speaker;
        private static bool initalized = false;

        private static Dictionary<Guid, AudioSource> genericSources = new Dictionary<Guid, AudioSource>();
        private static Dictionary<Guid, AudioSource> providedSources = new Dictionary<Guid, AudioSource>();

        private static GameObject go;

        private static bool loggedUnsupportedPlatform = false;
        private static bool loggedVPIsNull = false;

        private const int cleanupFramecount = 120;

        private static bool loggedOnlyOneInstance = false;

        private static char[] splitCharWords = new char[] { ' ' };

        #endregion

        #region Events

        public delegate void SpeakNativeCurrentWord(object sender, CurrentWordEventArgs e);
        public delegate void SpeakNativeCurrentPhoneme(object sender, CurrentPhonemeEventArgs e);
        public delegate void SpeakNativeCurrentViseme(object sender, CurrentVisemeEventArgs e);
        public delegate void SpeakNativeStart(object sender, SpeakNativeEventArgs e);
        public delegate void SpeakNativeComplete(object sender, SpeakNativeEventArgs e);
        public delegate void SpeakStart(object sender, SpeakEventArgs e);
        public delegate void SpeakComplete(object sender, SpeakEventArgs e);
        public delegate void SpeakAudioGenerationStart(object sender, SpeakEventArgs e);
        public delegate void SpeakAudioGenerationComplete(object sender, SpeakEventArgs e);
        public delegate void ErrorInfo(string info);

        /// <summary>An event triggered whenever a new word is spoken (native mode).</summary>
        public static event SpeakNativeCurrentWord OnSpeakNativeCurrentWord;

        /// <summary>An event triggered whenever a new phoneme is spoken (native mode).</summary>
        public static event SpeakNativeCurrentPhoneme OnSpeakNativeCurrentPhoneme;

        /// <summary>An event triggered whenever a new viseme is spoken  (native mode).</summary>
        public static event SpeakNativeCurrentViseme OnSpeakNativeCurrentViseme;

        /// <summary>An event triggered whenever a native speak is started.</summary>
        public static event SpeakNativeStart OnSpeakNativeStart;

        /// <summary>An event triggered whenever a native speak is completed.</summary>
        public static event SpeakNativeComplete OnSpeakNativeComplete;

        /// <summary>An event triggered whenever a speak is started.</summary>
        public static event SpeakStart OnSpeakStart;

        /// <summary>An event triggered whenever a native speak is completed.</summary>
        public static event SpeakComplete OnSpeakComplete;

        /// <summary>An event triggered whenever a speak audio generation is started.</summary>
        public static event SpeakAudioGenerationStart OnSpeakAudioGenerationStart;

        /// <summary>An event triggered whenever a speak audio generation is completed.</summary>
        public static event SpeakAudioGenerationComplete OnSpeakAudioGenerationComplete;

        /// <summary>An event triggered whenever an error occurs.</summary>
        public static event ErrorInfo OnErrorInfo;

        #endregion

        #region MonoBehaviour methods

        void OnEnable()
        {
            if (Helper.isEditorMode || !initalized)
            {
                go = gameObject;

                go.name = Constants.RTVOICE_SCENE_OBJECT_NAME;

                if (Helper.isWindowsPlatform)
                {
                    voiceProvider = new VoiceProviderWindows();
                }
                else if (Helper.isAndroidPlatform)
                {
                    voiceProvider = new VoiceProviderAndroid();
                }
                else if (Helper.isIOSPlatform)
                {
                    voiceProvider = new VoiceProviderIOS();
                    //List<Voice> voice = voiceProvider.Voices;
                }
                else
                { // always add a default provider
                    voiceProvider = new VoiceProviderMacOS();
                }

                // Subscribe event listeners
                BaseVoiceProvider.OnSpeakNativeCurrentWord += onSpeakNativeCurrentWord;
                BaseVoiceProvider.OnSpeakNativeCurrentPhoneme += onSpeakNativeCurrentPhoneme;
                BaseVoiceProvider.OnSpeakNativeCurrentViseme += onSpeakNativeCurrentViseme;
                BaseVoiceProvider.OnSpeakNativeStart += onSpeakNativeStart;
                BaseVoiceProvider.OnSpeakNativeComplete += onSpeakNativeComplete;
                BaseVoiceProvider.OnSpeakStart += onSpeakStart;
                BaseVoiceProvider.OnSpeakComplete += onSpeakComplete;
                BaseVoiceProvider.OnSpeakAudioGenerationStart += onSpeakAudioGenerationStart;
                BaseVoiceProvider.OnSpeakAudioGenerationComplete += onSpeakAudioGenerationComplete;
                BaseVoiceProvider.OnErrorInfo += onErrorInfo;

                speaker = this;

                if (!Helper.isEditorMode)
                {
                    DontDestroyOnLoad(transform.root.gameObject);
                    initalized = true;
                }
            }
            else
            {
                if (!Helper.isEditorMode)
                {
                    if (!loggedOnlyOneInstance)
                    {
                        Debug.LogWarning("Only one active instance of 'RTVoice' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");

                        Destroy(gameObject, 0.2f);
                    }
                }
            }
        }

        void Update()
        {
            if (Time.frameCount % cleanupFramecount == 0)
            {
                if (genericSources.Count > 0)
                {
                    foreach (KeyValuePair<Guid, AudioSource> source in genericSources)
                    {
                        if (source.Value != null && source.Value.clip != null && !source.Value.isPlaying)
                        {
                            removeSources.Add(source.Key, source.Value);
                        }
                    }

                    foreach (KeyValuePair<Guid, AudioSource> source in removeSources)
                    {
                        genericSources.Remove(source.Key);
                        Destroy(source.Value);
                    }

                    removeSources.Clear();
                }

                if (providedSources.Count > 0)
                {
                    foreach (KeyValuePair<Guid, AudioSource> source in providedSources)
                    {
                        if (source.Value != null && source.Value.clip != null && !source.Value.isPlaying)
                        {
                            removeSources.Add(source.Key, source.Value);
                        }
                    }

                    foreach (KeyValuePair<Guid, AudioSource> source in removeSources)
                    {
                        genericSources.Remove(source.Key);
                    }

                    removeSources.Clear();
                }
            }

            if (Helper.isEditorMode)
            {
                if (go != null)
                {
                    go.name = Constants.RTVOICE_SCENE_OBJECT_NAME; //ensure name
                }
            }
        }

        void OnDestroy()
        {
            if (voiceProvider != null)
            {
                voiceProvider.Silence();

                // Unsubscribe event listeners
                BaseVoiceProvider.OnSpeakNativeCurrentWord -= onSpeakNativeCurrentWord;
                BaseVoiceProvider.OnSpeakNativeCurrentPhoneme -= onSpeakNativeCurrentPhoneme;
                BaseVoiceProvider.OnSpeakNativeCurrentViseme -= onSpeakNativeCurrentViseme;
                BaseVoiceProvider.OnSpeakNativeStart -= onSpeakNativeStart;
                BaseVoiceProvider.OnSpeakNativeComplete -= onSpeakNativeComplete;
                BaseVoiceProvider.OnSpeakStart -= onSpeakStart;
                BaseVoiceProvider.OnSpeakComplete -= onSpeakComplete;
                BaseVoiceProvider.OnSpeakAudioGenerationStart -= onSpeakAudioGenerationStart;
                BaseVoiceProvider.OnSpeakAudioGenerationComplete -= onSpeakAudioGenerationComplete;
                BaseVoiceProvider.OnErrorInfo -= onErrorInfo;
            }
        }

        void OnApplicationQuit()
        {
            if (voiceProvider != null)
            {
                if (speaker != null)
                {
                    speaker.StopAllCoroutines();
                }

                voiceProvider.Silence();

                if (Helper.isAndroidPlatform)
                {
                    ((VoiceProviderAndroid)voiceProvider).ShutdownTTS();
                }
            }
        }

        #endregion

        #region Static properties

        /// <summary>Returns the extension of the generated audio files.</summary>
        /// <returns>Extension of the generated audio files.</returns>
        public static string AudioFileExtension
        {
            get
            {
                if (Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.AudioFileExtension;
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return string.Empty;
            }
        }

        /// <summary>Get all available voices from the current TTS-system.</summary>
        /// <returns>All available voices (alphabetically ordered by 'Name') as a list.</returns>
        public static List<Voice> Voices
        {
            get
            {
                if (Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.Voices.OrderBy(s => s.Name).ToList();
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return new List<Voice>();
            }
        }

        /// <summary>Get all available cultures from the current TTS-system..</summary>
        /// <returns>All available cultures (alphabetically ordered by 'Culture') as a list.</returns>
        public static List<string> Cultures
        {
            get
            {
                List<string> result = new List<string>();

                if (Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        IEnumerable<Voice> cultures = voiceProvider.Voices.GroupBy(cul => cul.Culture).Select(grp => grp.First()).OrderBy(s => s.Culture).ToList();

                        foreach (Voice voice in cultures)
                        {
                            result.Add(voice.Culture);
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return result;
            }
        }

        /// <summary>Checks if TTS is available on this system.</summary>
        /// <returns>True if TTS is available on this system.</returns>
        public static bool isTTSAvailable
        {
            get
            {
                if (Helper.isSupportedPlatform)
                {
                    if (voiceProvider != null)
                    {
                        return voiceProvider.Voices.Count > 0;
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logUnsupportedPlatform();
                }

                return false;
            }
        }

        #endregion


        #region Static methods

        /// <summary>
        /// Approximates the speech length in seconds of a given text and rate. 
        /// Note: This is an experimental method and doesn't provide an exact value; +/- 15% is "normal"!
        /// </summary>
        /// <param name="text">Text for the length approximation.</param>
        /// <param name="rate">Speech rate of the speaker in percent for the length approximation (1 = 100%, default: 1, optional).</param>
        /// <param name="wordsPerMinute">Words per minute (default: 175, optional).</param>
        /// <param name="timeFactor">Time factor for the calculated value (default: 0.9, optional).</param>
        /// <returns>Approximated speech length in seconds of the given text and rate.</returns>
        public static float ApproximateSpeechLength(string text, float rate = 1f, float wordsPerMinute = 175f, float timeFactor = 0.9f)
        {
            float words = (float)text.Split(splitCharWords, StringSplitOptions.RemoveEmptyEntries).Length;
            float characters = (float)text.Length - words + 1;
            float ratio = characters / words;

            //Debug.Log("words: " + words);
            //Debug.Log("characters: " + characters);
            //Debug.Log("ratio: " + ratio);

            if (Helper.isWindowsPlatform)
            {
                if (rate != 1f)
                { //relevant?
                    if (rate > 1f)
                    { //larger than 1
                        if (rate >= 2.75f)
                        {
                            rate = 2.78f;
                        }
                        else if (rate >= 2.6f && rate < 2.75f)
                        {
                            rate = 2.6f;
                        }
                        else if (rate >= 2.35f && rate < 2.6f)
                        {
                            rate = 2.39f;
                        }
                        else if (rate >= 2.2f && rate < 2.35f)
                        {
                            rate = 2.2f;
                        }
                        else if (rate >= 2f && rate < 2.2f)
                        {
                            rate = 2f;
                        }
                        else if (rate >= 1.8f && rate < 2f)
                        {
                            rate = 1.8f;
                        }
                        else if (rate >= 1.6f && rate < 1.8f)
                        {
                            rate = 1.6f;
                        }
                        else if (rate >= 1.4f && rate < 1.6f)
                        {
                            rate = 1.45f;
                        }
                        else if (rate >= 1.2f && rate < 1.4f)
                        {
                            rate = 1.28f;
                        }
                        else if (rate > 1f && rate < 1.2f)
                        {
                            rate = 1.14f;
                        }
                    }
                    else
                    { //smaller than 1
                        if (rate <= 0.3f)
                        {
                            rate = 0.33f;
                        }
                        else if (rate > 0.3 && rate <= 0.4f)
                        {
                            rate = 0.375f;
                        }
                        else if (rate > 0.4 && rate <= 0.45f)
                        {
                            rate = 0.42f;
                        }
                        else if (rate > 0.45 && rate <= 0.5f)
                        {
                            rate = 0.47f;
                        }
                        else if (rate > 0.5 && rate <= 0.55f)
                        {
                            rate = 0.525f;
                        }
                        else if (rate > 0.55 && rate <= 0.6f)
                        {
                            rate = 0.585f;
                        }
                        else if (rate > 0.6 && rate <= 0.7f)
                        {
                            rate = 0.655f;
                        }
                        else if (rate > 0.7 && rate <= 0.8f)
                        {
                            rate = 0.732f;
                        }
                        else if (rate > 0.8 && rate <= 0.9f)
                        {
                            rate = 0.82f;
                        }
                        else if (rate > 0.9 && rate < 1f)
                        {
                            rate = 0.92f;
                        }
                    }
                }
            }

            float speechLength = words / ((wordsPerMinute / 60) * rate);

            //Debug.Log("speechLength before: " + speechLength);

            if (ratio < 2)
            {
                speechLength *= 1f;
            }
            else if (ratio >= 2f && ratio < 3f)
            {
                speechLength *= 1.05f;
            }
            else if (ratio >= 3f && ratio < 3.5f)
            {
                speechLength *= 1.15f;
            }
            else if (ratio >= 3.5f && ratio < 4f)
            {
                speechLength *= 1.2f;
            }
            else if (ratio >= 4f && ratio < 4.5f)
            {
                speechLength *= 1.25f;
            }
            else if (ratio >= 4.5f && ratio < 5f)
            {
                speechLength *= 1.3f;
            }
            else if (ratio >= 5f && ratio < 5.5f)
            {
                speechLength *= 1.4f;
            }
            else if (ratio >= 5.5f && ratio < 6f)
            {
                speechLength *= 1.45f;
            }
            else if (ratio >= 6f && ratio < 6.5f)
            {
                speechLength *= 1.5f;
            }
            else if (ratio >= 6.5f && ratio < 7f)
            {
                speechLength *= 1.6f;
            }
            else if (ratio >= 7f && ratio < 8f)
            {
                speechLength *= 1.7f;
            }
            else if (ratio >= 8f && ratio < 9f)
            {
                speechLength *= 1.8f;
            }
            else
            {
                speechLength *= ((ratio * ((ratio / 100f) + 0.02f)) + 1f);
            }

            if (speechLength < 0.8f)
            {
                speechLength += 0.6f;
            }

            //Debug.Log("speechLength after: " + speechLength);

            return speechLength * timeFactor;
        }

        /// <summary>Get all available voices for a given culture from the current TTS-system.</summary>
        /// <param name="culture">Culture of the voice (e.g. "en")</param>
        /// <returns>All available voices (alphabetically ordered by 'Name') for a given culture as a list.</returns>
        public static List<Voice> VoicesForCulture(string culture)
        {
            if (Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (string.IsNullOrEmpty(culture))
                    {
                        Debug.LogWarning("The given 'culture' is null or empty! Returning all available voices.");

                        return voiceProvider.Voices;
                    }
                    else
                    {
                        return voiceProvider.Voices.Where(s => s.Culture.StartsWith(culture, StringComparison.InvariantCultureIgnoreCase)).OrderBy(s => s.Name).ToList();
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }

            return new List<Voice>();
        }

        /// <summary>Get a voice from for a given culture and otional index from the current TTS-system.</summary>
        /// <param name="culture">Culture of the voice (e.g. "en")</param>
        /// <param name="index">Index of the voice (default = 0, optional)</param>
        /// <returns>Voice for the given culture and index.</returns>
        public static Voice VoiceForCulture(string culture, int index = 0)
        {
            Voice result = null;

            if (!string.IsNullOrEmpty(culture))
            {
                List<Voice> voices = VoicesForCulture(culture);

                if (voices.Count > 0)
                {
                    if (voices.Count - 1 >= index && index >= 0)
                    {
                        result = voices[index];
                    }
                    else
                    {
                        result = voices[0];
                        Debug.LogWarning("No voices for culture '" + culture + "' with index '" + index + "' found! Speaking with the default voice!");
                    }
                }
                else
                { //use the default voice
                    Debug.LogWarning("No voice for culture '" + culture + "' found! Speaking with the default voice!");
                    //result = null;
                }
            }
            return result;
        }

        /// <summary>Get a voice for a given name from the current TTS-system.</summary>
        /// <param name="name">Name of the voice (e.g. "Alex")</param>
        /// <returns>Voice for the given name or null if not found.</returns>
        public static Voice VoiceForName(string name)
        {
            Voice result = null;

            if (Helper.isSupportedPlatform)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogWarning("The given 'name' is null or empty! Returning null.");
                }
                else
                {
                    if (voiceProvider != null)
                    {
                        foreach (Voice voice in voiceProvider.Voices)
                        {
                            if (name.Equals(voice.Name))
                            {
                                result = voice;
                                break;
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
            }
            else
            {
                logUnsupportedPlatform();
            }

            return result;
        }

        /// <summary>Speaks a text with a given voice (native mode).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        /// <returns>UID of the speaker.</returns>
        public static Guid SpeakNative(string text, Voice voice = null, float rate = 1f, float volume = 1f, float pitch = 1f)
        {
            WrapperNative wrapper = new WrapperNative(text, voice, rate, pitch, volume);

            SpeakNativeWithUID(wrapper);

            return wrapper.Uid;
        }

        /// <summary>Speaks a text with a given voice (native mode).</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        public static void SpeakNativeWithUID(WrapperNative wrapper)
        {
            if (Helper.isSupportedPlatform)
            {
                if (wrapper != null)
                {
                    if (voiceProvider != null)
                    {
                        if (string.IsNullOrEmpty(wrapper.Text))
                        {
                            Debug.LogWarning("'Text' is null or empty!");
                        }
                        else
                        {
                            if (speaker != null)
                            {
                                speaker.StartCoroutine(voiceProvider.SpeakNative(wrapper));
                                //                     } else {
                                //                        logSpeakerIsNull();
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logWrapperIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Speaks a text with a given wrapper (native mode).</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        /// <returns>UID of the speaker.</returns>
        public static Guid SpeakNative(WrapperNative wrapper)
        {
            if (wrapper != null)
            {
                SpeakNativeWithUID(wrapper);

                return wrapper.Uid;
            }
            else
            {
                logWrapperIsNull();
            }

            return Guid.NewGuid(); //fake uid
        }

        /// <summary>Speaks a text with a given voice.</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="source">AudioSource for the output (optional).</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="speakImmediately">Speak the text immediately (default: true). Only works if 'Source' is not null.</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>///
        /// <param name="outputFile">Saves the generated audio to an output file (without extension, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        /// <returns>UID of the speaker.</returns>
        public static Guid Speak(string text, AudioSource source = null, Voice voice = null, bool speakImmediately = true, float rate = 1f, float volume = 1f, string outputFile = "", float pitch = 1f)
        {
            Wrapper wrapper = new Wrapper(text, source, voice, speakImmediately, rate, pitch, volume, outputFile);

            SpeakWithUID(wrapper);

            return wrapper.Uid;
        }

        /// <summary>Speaks a text with a given voice.</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        public static void SpeakWithUID(Wrapper wrapper)
        {
            if (Helper.isSupportedPlatform)
            {
                if (wrapper != null)
                {
                    if (voiceProvider != null)
                    {
                        if (string.IsNullOrEmpty(wrapper.Text))
                        {
                            Debug.LogWarning("'Text' is null or empty!");
                        }
                        else
                        {
                            if (wrapper.Source == null)
                            {
                                wrapper.Source = go.AddComponent<AudioSource>();
                                genericSources.Add(wrapper.Uid, wrapper.Source);
                                wrapper.SpeakImmediately = true; //must always speak immediately (since there is no AudioSource given)
                            }
                            else
                            {
                                providedSources.Add(wrapper.Uid, wrapper.Source);
                            }

                            if (speaker != null)
                            {
                                speaker.StartCoroutine(voiceProvider.Speak(wrapper));
                                //                     } else {
                                //                        logSpeakerIsNull();
                            }
                        }
                    }
                    else
                    {
                        logVPIsNull();
                    }
                }
                else
                {
                    logWrapperIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Speaks a text with a given wrapper.</summary>
        /// <param name="wrapper">Speak wrapper.</param>
        /// <returns>UID of the speaker.</returns>
        public static Guid Speak(Wrapper wrapper)
        {
            if (wrapper != null)
            {
                SpeakWithUID(wrapper);

                return wrapper.Uid;
            }
            else
            {
                logWrapperIsNull();
            }

            return Guid.NewGuid(); //fake uid
        }

        /// <summary>Speaks a text with a given voice and tracks the word position.</summary>
        /// <param name="uid">UID of the speaker</param>
        /// <param name="text">Text to speak.</param>
        /// <param name="source">AudioSource for the output.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech in percent (1 = 100%, default: 1, optional).</param>
        public static void SpeakMarkedWordsWithUID(Guid uid, string text, AudioSource source, Voice voice = null, float rate = 1f, float pitch = 1f)
        { //TODO wrapper-version?
            if (Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        Debug.LogWarning("The given 'text' is null or empty!");
                    }
                    else
                    {
                        AudioSource src = source;

                        if (src == null || src.clip == null)
                        {
                            Debug.LogError("'source' must be a valid AudioSource with a clip! Use 'Speak()' before!");
                        }
                        else
                        {
                            SpeakNativeWithUID(new WrapperNative(uid, text, voice, rate, pitch, 0));
                            if (!Helper.isMacOSPlatform)
                            { //prevent "double-speak" on OSX
                                src.Play();
                            }
                        }
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        //      /// <summary>
        //      /// Speaks a text with a given voice and tracks the word position.
        //      /// </summary>
        //      public static Guid SpeakMarkedWords(string text, AudioSource source = null, Voice voice = null, int rate = 1, int volume = 100) {
        //         Guid result = Guid.NewGuid();
        //
        //         SpeakMarkedWordsWithUID(result, text, source, voice, rate, volume);
        //
        //         return result;
        //      }

        /// <summary>Silence all active TTS-voices.</summary>
        public static void Silence()
        {
            if (Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (speaker != null)
                    {
                        speaker.StopAllCoroutines();
                    }

                    voiceProvider.Silence();

                    foreach (KeyValuePair<Guid, AudioSource> source in genericSources)
                    {
                        if (source.Value != null)
                        {
                            source.Value.Stop();
                            Destroy(source.Value, 0.1f);
                        }
                    }
                    genericSources.Clear();

                    foreach (KeyValuePair<Guid, AudioSource> source in providedSources)
                    {
                        if (source.Value != null)
                        {
                            source.Value.Stop();
                        }
                    }
                }
                else
                {
                    providedSources.Clear();

                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        /// <summary>Silence an active TTS-voice with a UID.</summary>
        /// <param name="uid">UID of the speaker</param>
        public static void Silence(Guid uid)
        {
            if (Helper.isSupportedPlatform)
            {
                if (voiceProvider != null)
                {
                    if (genericSources.ContainsKey(uid))
                    {
                        AudioSource source;

                        if (genericSources.TryGetValue(uid, out source))
                        {
                            source.Stop();
                            genericSources.Remove(uid);
                        }
                    }
                    else if (providedSources.ContainsKey(uid))
                    {
                        AudioSource source;

                        if (providedSources.TryGetValue(uid, out source))
                        {
                            source.Stop();
                            providedSources.Remove(uid);
                        }
                    }
                    else
                    {
                        voiceProvider.Silence(uid);
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                logUnsupportedPlatform();
            }
        }

        private static void logWrapperIsNull()
        {
            //if (!loggedWrapperIsNull) {
            string errorMessage = "'wrapper' is null!";
            Debug.LogError(errorMessage);
            onErrorInfo(errorMessage);
            //}
        }

        private static void logUnsupportedPlatform()
        {
            string errorMessage = "RTVoice is not supported on your platform!";

            onErrorInfo(errorMessage);

            if (!loggedUnsupportedPlatform)
            {
                Debug.LogError(errorMessage);
                loggedUnsupportedPlatform = true;
            }
        }

        private static void logVPIsNull()
        {
            string errorMessage = "'voiceProvider' is null!" + Environment.NewLine + "Did you add the 'RTVoice'-prefab to the current scene?";

            onErrorInfo(errorMessage);

            if (!loggedVPIsNull)
            {
                Debug.LogWarning(errorMessage);
                loggedVPIsNull = true;
            }
        }
        #endregion

        #region Private methods

        private void onSpeakNativeCurrentWord(CurrentWordEventArgs e)
        {
            if (OnSpeakNativeCurrentWord != null)
            {
                OnSpeakNativeCurrentWord(this, e);
            }
        }

        private void onSpeakNativeCurrentPhoneme(CurrentPhonemeEventArgs e)
        {
            if (OnSpeakNativeCurrentPhoneme != null)
            {
                OnSpeakNativeCurrentPhoneme(this, e);
            }
        }

        private void onSpeakNativeCurrentViseme(CurrentVisemeEventArgs e)
        {
            if (OnSpeakNativeCurrentViseme != null)
            {
                OnSpeakNativeCurrentViseme(this, e);
            }
        }

        private void onSpeakNativeStart(SpeakNativeEventArgs e)
        {
            if (OnSpeakNativeStart != null)
            {
                OnSpeakNativeStart(this, e);
            }
        }

        private void onSpeakNativeComplete(SpeakNativeEventArgs e)
        {
            if (OnSpeakNativeComplete != null)
            {
                OnSpeakNativeComplete(this, e);
            }
        }

        private void onSpeakStart(SpeakEventArgs e)
        {
            if (OnSpeakStart != null)
            {
                OnSpeakStart(this, e);
            }
        }

        private void onSpeakComplete(SpeakEventArgs e)
        {
            if (OnSpeakComplete != null)
            {
                OnSpeakComplete(this, e);
            }
        }

        private void onSpeakAudioGenerationStart(SpeakEventArgs e)
        {
            if (OnSpeakAudioGenerationStart != null)
            {
                OnSpeakAudioGenerationStart(this, e);
            }
        }

        private void onSpeakAudioGenerationComplete(SpeakEventArgs e)
        {
            if (OnSpeakAudioGenerationComplete != null)
            {
                OnSpeakAudioGenerationComplete(this, e);
            }
        }

        private static void onErrorInfo(string errorInfo)
        {
            if (OnErrorInfo != null)
            {
                OnErrorInfo(errorInfo);
            }
        }

        #endregion

        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>Speaks a text with a given voice (native mode & Editor only).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="pitch">Pitch of the speech (1 = 100%, default: 1, optional).</param>
        public static void SpeakNativeInEditor(string text, Voice voice = null, float rate = 1f, float volume = 1f, float pitch = 1f)
        {
            if (Helper.isEditorMode)
            {
                if (voiceProvider != null)
                {
                    WrapperNative wrapper = new WrapperNative(text, voice, rate, pitch, volume);

                    if (string.IsNullOrEmpty(wrapper.Text))
                    {
                        Debug.LogWarning("'Text' is null or empty!");
                    }
                    else
                    {
                        Thread worker = new Thread(() => voiceProvider.SpeakNativeInEditor(wrapper));
                        worker.Start();
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                Debug.LogWarning("'SpeakNativeInEditor()' works only inside the Unity Editor!");
            }
        }

        /// <summary>Generates audio for a text with a given voice (Editor only).</summary>
        /// <param name="text">Text to speak.</param>
        /// <param name="voice">Voice to speak (optional).</param>
        /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, default: 1, optional).</param>
        /// <param name="volume">Volume of the speaker in percent (1 = 100%, default: 1, optional).</param>///
        /// <param name="outputFile">Saves the generated audio to an output file (without extension, optional).</param>
        /// <param name="pitch">Pitch of the speech (1 = 100%, default: 1, optional).</param>
        public static void GenerateInEditor(string text, Voice voice = null, float rate = 1f, float volume = 1f, string outputFile = "", float pitch = 1f)
        {
            if (Helper.isEditorMode)
            {
                if (voiceProvider != null)
                {
                    Wrapper wrapper = new Wrapper(text, null, voice, true, rate, pitch, volume, outputFile);

                    if (string.IsNullOrEmpty(wrapper.Text))
                    {
                        Debug.LogWarning("'Text' is null or empty!");
                    }
                    else
                    {
                        Thread worker = new Thread(() => voiceProvider.GenerateInEditor(wrapper));
                        worker.Start();
                    }
                }
                else
                {
                    logVPIsNull();
                }
            }
            else
            {
                Debug.LogWarning("'GenerateInEditor()' works only inside the Unity Editor!");
            }
        }

#endif

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com