using UnityEngine;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>FFT analyzer for an audio channel.</summary>
    public class FFTAnalyzer : MonoBehaviour
    {

        #region Variables

        public float[] Samples = new float[256];
        public int Channel = 0;
        public FFTWindow FFTMode = FFTWindow.BlackmanHarris;

        #endregion

        #region MonoBehaviour methods

        void Update()
        {
            AudioListener.GetSpectrumData(Samples, Channel, FFTMode);
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com