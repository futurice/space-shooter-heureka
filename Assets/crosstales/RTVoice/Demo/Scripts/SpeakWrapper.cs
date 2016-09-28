using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice.Model;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Warapper for the dynamic speakers.</summary>
    [RequireComponent(typeof(AudioSource))]
    public class SpeakWrapper : MonoBehaviour
    {

        #region Variables

        public Voice SpeakerVoice;
        public InputField Input;
        public Text Label;
        public AudioSource Audio;

        #endregion

        #region MonoBehaviour methods

        public void Start()
        {
            Audio = GetComponent<AudioSource>();
        }

        #endregion

        #region Public methods

        public void Speak()
        {
            if (GUISpeech.isNative)
            {
                Speaker.SpeakNative(Input.text, SpeakerVoice, GUISpeech.Rate, GUISpeech.Volume, GUISpeech.Pitch);
            }
            else
            {
                Speaker.Speak(Input.text, Audio, SpeakerVoice, true, GUISpeech.Rate, GUISpeech.Volume, "", GUISpeech.Pitch);
            }
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com