using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple GUI for audio filters on multiple objects.</summary>
    public class GUIMultiAudioFilter : MonoBehaviour
    {

        #region Variables

        public List<AudioSource> Sources = new List<AudioSource>();

        public List<AudioReverbFilter> ReverbFilters = new List<AudioReverbFilter>();
        public List<AudioChorusFilter> ChorusFilters = new List<AudioChorusFilter>();
        public List<AudioEchoFilter> EchoFilters = new List<AudioEchoFilter>();
        public List<AudioDistortionFilter> DistortionFilters = new List<AudioDistortionFilter>();
        public List<AudioLowPassFilter> LowPassFilters = new List<AudioLowPassFilter>();
        public List<AudioHighPassFilter> HighPassFilters = new List<AudioHighPassFilter>();

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
        }

        #endregion

        #region Public methods

        public void ResetFilters()
        {
            foreach (AudioSource source in Sources)
            {
                source.volume = 1f;
                source.pitch = 1f;
            }

            foreach (AudioReverbFilter reverbFilter in ReverbFilters)
            {
                reverbFilter.reverbPreset = reverbPresets[0];
            }

            foreach (AudioChorusFilter chorusFilter in ChorusFilters)
            {
                chorusFilter.enabled = false;
            }

            foreach (AudioEchoFilter echoFilter in EchoFilters)
            {
                echoFilter.enabled = false;
            }

            foreach (AudioDistortionFilter distortionFilter in DistortionFilters)
            {
                distortionFilter.distortionLevel = 0.5f;
                distortionFilter.enabled = false;
            }

            foreach (AudioLowPassFilter lowPassFilter in LowPassFilters)
            {
                lowPassFilter.cutoffFrequency = 5000;
                lowPassFilter.enabled = false;
            }

            foreach (AudioHighPassFilter highPassFilter in HighPassFilters)
            {
                highPassFilter.cutoffFrequency = 5000;
                highPassFilter.enabled = false;
            }

        }

        public void ClearFilters()
        {
            Sources.Clear();
            ReverbFilters.Clear();
            ChorusFilters.Clear();
            EchoFilters.Clear();
            DistortionFilters.Clear();
            LowPassFilters.Clear();
            HighPassFilters.Clear();
        }

        public void ReverbFilterDropdownChanged(Int32 index)
        {
            foreach (AudioReverbFilter reverbFilter in ReverbFilters)
            {
                reverbFilter.reverbPreset = reverbPresets[index];
            }
        }

        public void ChorusFilterEnabled(bool enabled)
        {
            foreach (AudioChorusFilter chorusFilter in ChorusFilters)
            {
                chorusFilter.enabled = enabled;
            }
        }

        public void EchoFilterEnabled(bool enabled)
        {
            foreach (AudioEchoFilter echoFilter in EchoFilters)
            {
                echoFilter.enabled = enabled;
            }
        }

        public void DistortionFilterEnabled(bool enabled)
        {
            foreach (AudioDistortionFilter distortionFilter in DistortionFilters)
            {
                distortionFilter.enabled = enabled;
            }
        }

        public void DistortionFilterChanged(float value)
        {
            foreach (AudioDistortionFilter distortionFilter in DistortionFilters)
            {
                distortionFilter.distortionLevel = value;
            }
            Distortion.text = value.ToString("0.00");
        }

        public void LowPassFilterEnabled(bool enabled)
        {
            foreach (AudioLowPassFilter lowPassFilter in LowPassFilters)
            {
                lowPassFilter.enabled = enabled;
            }
        }

        public void LowPassFilterChanged(float value)
        {
            foreach (AudioLowPassFilter lowPassFilter in LowPassFilters)
            {
                lowPassFilter.cutoffFrequency = value;
            }
            Lowpass.text = value.ToString();
        }

        public void HighPassFilterEnabled(bool enabled)
        {
            foreach (AudioHighPassFilter highPassFilter in HighPassFilters)
            {
                highPassFilter.enabled = enabled;
            }
        }

        public void HighPassFilterChanged(float value)
        {
            foreach (AudioHighPassFilter highPassFilter in HighPassFilters)
            {
                highPassFilter.cutoffFrequency = value;
            }
            Highpass.text = value.ToString();
        }

        public void VolumeChanged(float value)
        {
            foreach (AudioSource source in Sources)
            {
                source.volume = value;
            }
            Volume.text = value.ToString("0.00");
        }

        public void PitchChanged(float value)
        {
            foreach (AudioSource source in Sources)
            {
                source.pitch = value;
            }
            Pitch.text = value.ToString("0.00");
        }

        #endregion

    }
}
// Copyright 2016 www.crosstales.com