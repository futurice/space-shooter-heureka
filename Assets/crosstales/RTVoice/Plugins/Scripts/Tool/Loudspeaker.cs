using UnityEngine;

namespace Crosstales.RTVoice.Tool
{
    /// <summary>Loudspeaker for an AudioSource.</summary>
    [RequireComponent(typeof(AudioSource))]
    [HelpURL("http://www.crosstales.com/en/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_tool_1_1_loudspeaker.html")]
    public class Loudspeaker : MonoBehaviour
    {

        #region Variables

        /// <summary>Origin AudioSource.</summary>
        [Tooltip("Origin AudioSource.")]
        public AudioSource Source;

        /// <summary>Synchronized with the origin (default: on).</summary>
        [Tooltip("Synchronized with the origin (default: on).")]
        public bool Synchronized = true;

        /// <summary>Silence the origin (default: off).</summary>
        [Tooltip("Silence the origin (default: off).")]
        public bool SilenceSource = false;

        private AudioSource audioSource;

        private bool stopped = true;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.Stop(); //always stop the AudioSource at startup
        }

        void Update()
        {
            if (Source != null && Source.clip != null && Source.isPlaying)
            {
                if (stopped)
                {
                    audioSource.loop = Source.loop;
                    audioSource.clip = Source.clip;
                    //audioSource.timeSamples = Source.timeSamples;
                    audioSource.Play();

                    stopped = false;

                    if (SilenceSource)
                    {
                        Source.volume = 0f;
                    }
                }

                if (Synchronized)
                {
                    audioSource.timeSamples = Source.timeSamples;
                }
            }
            else
            {
                if (!stopped)
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                    stopped = true;
                }
            }
        }

        void OnDisable()
        {
            audioSource.Stop();
            audioSource.clip = null;
            stopped = true;
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com