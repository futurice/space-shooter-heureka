using UnityEngine;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo.Util
{
    /// <summary>Simple audio visualizer.</summary>
    public class AudioVisualizer : MonoBehaviour
    {

        #region Variables
        public FFTAnalyzer Analyzer;
        public GameObject VisualPrefab;
        public float Width = 0.075f;
        public float Gain = 70f;
        public bool LeftToRight = true;

        private Transform tf;
        private Transform[] visualTransforms;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            tf = transform;

            visualTransforms = new Transform[Analyzer.Samples.Length / 2];

            GameObject tempCube;

            for (int ii = 0; ii < Analyzer.Samples.Length / 2; ii++)
            { //cut the upper frequencies
                if (LeftToRight)
                {
                    tempCube = (GameObject)Instantiate(VisualPrefab, new Vector3(tf.position.x + (ii * Width), tf.position.y, tf.position.z), Quaternion.identity);
                }
                else
                {
                    tempCube = (GameObject)Instantiate(VisualPrefab, new Vector3(tf.position.x - (ii * Width), tf.position.y, tf.position.z), Quaternion.identity);
                }

                tempCube.GetComponent<Renderer>().material.color = Helper.HSVToRGB((360f / (Analyzer.Samples.Length / 2)) * ii, 1f, 1f, 1f);

                visualTransforms[ii] = tempCube.GetComponent<Transform>();
                visualTransforms[ii].parent = tf;
            }
        }

        void Update()
        {
            for (int ii = 0; ii < visualTransforms.Length; ii++)
            {
                visualTransforms[ii].localScale = new Vector3(Width, Analyzer.Samples[ii] * Gain, Width);
            }
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com