using UnityEngine;
using UnityEngine.UI;
using System;
using Crosstales.RTVoice.Model.Event;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>
    /// Simple TTS example.
    /// </summary>
    public class Simple : MonoBehaviour
    {

        #region Variables

        public AudioSource SourceA;
        public AudioSource SourceB;

        public Text TextSpeakerA;
        public Text TextSpeakerB;

        public Text PhonemeSpeakerA;
        public Text PhonemeSpeakerB;

        public Text VisemeSpeakerA;
        public Text VisemeSpeakerB;

        [Range(0f, 3f)]
        public float RateSpeakerA = 1.25f;
        [Range(0f, 3f)]
        public float RateSpeakerB = 1.75f;

        public bool PlayOnStart = false;

        private Guid uidSpeakerA;
        private Guid uidSpeakerB;

        private string textA;
        private string textB;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            // Subscribe event listeners
            Speaker.OnSpeakAudioGenerationStart += speakAudioGenerationStartMethod;
            Speaker.OnSpeakAudioGenerationComplete += speakAudioGenerationCompleteMethod;
            Speaker.OnSpeakNativeCurrentWord += speakNativeCurrentWordMethod;
            Speaker.OnSpeakNativeCurrentPhoneme += speakNativeCurrentPhonemeMethod;
            Speaker.OnSpeakNativeCurrentViseme += speakNativeCurrentVisemeMethod;
            Speaker.OnSpeakNativeStart += speakNativeStartMethod;
            Speaker.OnSpeakNativeComplete += speakNativeCompleteMethod;

            textA = TextSpeakerA.text;
            textB = TextSpeakerB.text;

            if (PlayOnStart)
            {
                Play();
            }
        }

        void OnDestroy()
        {
            // Unsubscribe event listeners
            Speaker.OnSpeakAudioGenerationStart -= speakAudioGenerationStartMethod;
            Speaker.OnSpeakAudioGenerationComplete -= speakAudioGenerationCompleteMethod;
            Speaker.OnSpeakNativeCurrentWord -= speakNativeCurrentWordMethod;
            Speaker.OnSpeakNativeCurrentPhoneme -= speakNativeCurrentPhonemeMethod;
            Speaker.OnSpeakNativeCurrentViseme -= speakNativeCurrentVisemeMethod;
            Speaker.OnSpeakNativeStart -= speakNativeStartMethod;
            Speaker.OnSpeakNativeComplete -= speakNativeCompleteMethod;
        }

        #endregion

        #region Public methods

        public void Play()
        {
            TextSpeakerA.text = textA;
            TextSpeakerB.text = textB;

            SpeakerA(); //start with speaker A
        }

        public void SpeakerA()
        { //Don't speak the text immediately
            uidSpeakerA = Speaker.Speak(TextSpeakerA.text, SourceA, Speaker.VoiceForCulture("en"), false, RateSpeakerA);
        }

        public void SpeakerB()
        { //Don't speak the text immediately
            uidSpeakerB = Speaker.Speak(TextSpeakerB.text, SourceB, Speaker.VoiceForCulture("en", 1), false, RateSpeakerB);
        }

        public void Silence()
        {
            Speaker.Silence();
            SourceA.Stop();
            SourceB.Stop();

            TextSpeakerA.text = textA;
            TextSpeakerB.text = textB;
            VisemeSpeakerB.text = PhonemeSpeakerB.text = VisemeSpeakerA.text = PhonemeSpeakerA.text = "-";
        }

        #endregion

        #region Callback methods

        private void speakAudioGenerationStartMethod(object sender, SpeakEventArgs e)
        {
            Debug.Log("SpeakAudioGenerationStartMethod: " + e);
        }

        private void speakAudioGenerationCompleteMethod(object sender, SpeakEventArgs e)
        {
            Speaker.SpeakMarkedWordsWithUID(e.Wrapper.Uid, e.Wrapper.Text, e.Wrapper.Source, e.Wrapper.Voice, e.Wrapper.Rate);
        }

        private void speakNativeStartMethod(object sender, SpeakNativeEventArgs e)
        {
            if (e.Wrapper.Uid.Equals(uidSpeakerA))
            {
                //Debug.Log("Speaker A - Speech start: " + e);
            }
            else if (e.Wrapper.Uid.Equals(uidSpeakerB))
            {
                //Debug.Log("Speaker B - Speech start: " + e);
            }
            else
            {
                Debug.LogWarning("Unknown speaker: " + e);
            }
        }

        private void speakNativeCompleteMethod(object sender, SpeakNativeEventArgs e)
        {
            if (e.Wrapper.Uid.Equals(uidSpeakerA))
            {
                //Debug.Log("Speaker A - Speech complete: " + e);
                TextSpeakerA.text = e.Wrapper.Text;
                VisemeSpeakerA.text = PhonemeSpeakerA.text = "-";

                SpeakerB();
            }
            else if (e.Wrapper.Uid.Equals(uidSpeakerB))
            {
                //Debug.Log("Speaker B - Speech complete: " + e);
                TextSpeakerB.text = e.Wrapper.Text;

                VisemeSpeakerB.text = PhonemeSpeakerB.text = "-";
            }
            else
            {
                Debug.LogWarning("Unknown speaker: " + e);
            }
        }

        private void speakNativeCurrentWordMethod(object sender, CurrentWordEventArgs e)
        {
            //Debug.Log(speechTextArray [wordIndex]);

            if (e.Wrapper.Uid.Equals(uidSpeakerA))
            {
                TextSpeakerA.text = Helper.MarkSpokenText(e.SpeechTextArray, e.WordIndex);
            }
            else if (e.Wrapper.Uid.Equals(uidSpeakerB))
            {
                TextSpeakerB.text = Helper.MarkSpokenText(e.SpeechTextArray, e.WordIndex);
            }
            else
            {
                Debug.LogWarning("Unknown speaker: " + e);
            }
        }

        private void speakNativeCurrentPhonemeMethod(object sender, CurrentPhonemeEventArgs e)
        {
            if (e.Wrapper.Uid.Equals(uidSpeakerA))
            {
                PhonemeSpeakerA.text = e.Phoneme;
            }
            else if (e.Wrapper.Uid.Equals(uidSpeakerB))
            {
                PhonemeSpeakerB.text = e.Phoneme;
            }
            else
            {
                Debug.LogWarning("Unknown speaker: " + e);
            }
        }

        private void speakNativeCurrentVisemeMethod(object sender, CurrentVisemeEventArgs e)
        {
            if (e.Wrapper.Uid.Equals(uidSpeakerA))
            {
                VisemeSpeakerA.text = e.Viseme;
            }
            else if (e.Wrapper.Uid.Equals(uidSpeakerB))
            {
                VisemeSpeakerB.text = e.Viseme;
            }
            else
            {
                Debug.LogWarning("Unknown speaker: " + e);
            }
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com