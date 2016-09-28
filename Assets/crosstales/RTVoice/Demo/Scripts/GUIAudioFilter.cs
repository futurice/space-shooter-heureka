using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple GUI for audio filters.</summary>
    public class GUIAudioFilter : MonoBehaviour
    {

        #region Variables

        public AudioSource Source;

        public AudioReverbFilter ReverbFilter;
        public AudioChorusFilter ChorusFilter;
        public AudioEchoFilter EchoFilter;
        public AudioDistortionFilter DistortionFilter;
        public AudioLowPassFilter LowPassFilter;
        public AudioHighPassFilter HighPassFilter;

        public Text Distortion;
        public Text Lowpass;
        public Text Highpass;
        public Text Volume;
        public Text Pitch;

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
        public Dropdown ReverbFilterDropdown;
#endif

        private List<AudioReverbPreset> reverbPresets = new List<AudioReverbPreset>();

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
#endif

            foreach (AudioReverbPreset arp in Enum.GetValues(typeof(AudioReverbPreset)))
            {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                options.Add(new Dropdown.OptionData(arp.ToString()));
#endif

                reverbPresets.Add(arp);
            }

#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            if (ReverbFilterDropdown != null)
            {
                ReverbFilterDropdown.ClearOptions();
                ReverbFilterDropdown.AddOptions(options);
            }
#endif

            ResetFilters();
        }

        #endregion

        #region Public methods

        public void ResetFilters()
        {
            Source.pitch = 1f;
            Source.volume = 1f;
            ReverbFilter.reverbPreset = reverbPresets[0];
            ChorusFilter.enabled = false;
            EchoFilter.enabled = false;
            DistortionFilter.distortionLevel = 0.5f;
            DistortionFilter.enabled = false;
            LowPassFilter.cutoffFrequency = 5000;
            LowPassFilter.enabled = false;
            HighPassFilter.cutoffFrequency = 5000;
            HighPassFilter.enabled = false;
        }

        public void ReverbFilterDropdownChanged(Int32 index)
        {
            ReverbFilter.reverbPreset = reverbPresets[index];
        }

        public void ChorusFilterEnabled(bool enabled)
        {
            ChorusFilter.enabled = enabled;
        }

        public void EchoFilterEnabled(bool enabled)
        {
            EchoFilter.enabled = enabled;
        }

        public void DistortionFilterEnabled(bool enabled)
        {
            DistortionFilter.enabled = enabled;
        }

        public void DistortionFilterChanged(float value)
        {
            DistortionFilter.distortionLevel = value;
            Distortion.text = value.ToString("0.00");
        }

        public void LowPassFilterEnabled(bool enabled)
        {
            LowPassFilter.enabled = enabled;
        }

        public void LowPassFilterChanged(float value)
        {
            LowPassFilter.cutoffFrequency = value;
            Lowpass.text = value.ToString();
        }

        public void HighPassFilterEnabled(bool enabled)
        {
            HighPassFilter.enabled = enabled;
        }

        public void HighPassFilterChanged(float value)
        {
            HighPassFilter.cutoffFrequency = value;
            Highpass.text = value.ToString();
        }

        public void VolumeChanged(float value)
        {
            Source.volume = value;
            Volume.text = value.ToString("0.00");
        }

        public void PitchChanged(float value)
        {
            Source.pitch = value;
            Pitch.text = value.ToString("0.00");
        }

        #endregion

    }
}
// Copyright 2016 www.crosstales.com