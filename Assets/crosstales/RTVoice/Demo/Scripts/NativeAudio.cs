using UnityEngine;
using Crosstales.RTVoice.Model.Event;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple example with native audio for exact timing.</summary>
    public class NativeAudio : MonoBehaviour
    {

        #region Variables

        public string SpeechText = "This is an example with native audio for exact timing (e.g. animations).";
        public bool PlayOnStart = false;
        public float Delay = 1f;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            // Subscribe event listeners
            Speaker.OnSpeakNativeStart += play;
            Speaker.OnSpeakNativeComplete += stop;

            if (PlayOnStart)
            {
                Invoke("StartTTS", Delay); //Invoke the TTS-system after x seconds
            }
        }

        void OnDestroy()
        {
            // Unsubscribe event listeners
            Speaker.OnSpeakNativeStart -= play;
            Speaker.OnSpeakNativeComplete -= stop;
        }

        #endregion

        #region Public methods

        public void StartTTS()
        {
            Speaker.SpeakNative(SpeechText, Speaker.VoiceForCulture("en", 1));
        }

        public void Silence()
        {
            Speaker.Silence();
        }

        #endregion

        #region Callback methods

        private void play(object sender, SpeakNativeEventArgs e)
        {
            Debug.Log("Play your animations to the event: " + e);

            //Here belongs your stuff, like animations
        }

        private void stop(object sender, SpeakNativeEventArgs e)
        {
            Debug.Log("Stop your animations from the event: " + e);

            //Here belongs your stuff, like animations
        }

        #endregion

    }
}
// Copyright 2016 www.crosstales.com