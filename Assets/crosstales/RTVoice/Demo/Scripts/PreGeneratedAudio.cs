using UnityEngine;
using Crosstales.RTVoice.Model.Event;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple example with pre-generated audio for exact timing.</summary>
    public class PreGeneratedAudio : MonoBehaviour
    {

        #region Variables

        public string SpeechText = "This is an example with pre-generated audio for exact timing (e.g. animations).";
        public bool PlayOnStart = false;
        //public float Delay = 1f;

        private AudioSource audioSource;
        private bool isPlayed = false;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();

            Speaker.OnSpeakAudioGenerationComplete += speakAudioGenerationCompleteMethod;

            Speaker.Speak(SpeechText, audioSource, Speaker.VoiceForCulture("en", 1), false);

            //         if(PlayOnStart) {
            //            Invoke("Play", Delay); //Invoke the audio source after x seconds
            //         }
        }

        void Update()
        {
            if (!audioSource.isPlaying && isPlayed)
            {
                Stop();
            }
        }

        void OnDestroy()
        {
            Speaker.OnSpeakAudioGenerationComplete -= speakAudioGenerationCompleteMethod;
        }

        #endregion

        #region Public methods

        public void Play()
        {
            Debug.Log("Play your animations!");

            isPlayed = true;

            audioSource.Play();

            //Here belongs your stuff, like animations
        }

        public void Silence()
        {
            audioSource.Stop();
        }

        public void Stop()
        {
            Debug.Log("Stop your animations!");

            isPlayed = false;

            //Here belongs your stuff, like animations
        }

        #endregion

        private void speakAudioGenerationCompleteMethod(object sender, SpeakEventArgs e)
        {
            if (PlayOnStart)
            {
                Play();
            }
        }
    }
}
// Copyright 2016 www.crosstales.com