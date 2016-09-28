using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>
    /// Simple Sequence caller example.
    /// </summary>
    public class SequenceCaller : MonoBehaviour
    {

        #region Variables

        public GameObject receiver;
        public int NumberOfSequences;
        public float SequenceDelay = 1f;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            //receiver.SendMessage("PlaySequence");

            for (int ii = 0; ii < NumberOfSequences; ii++)
            {
                Invoke("playNextSequence", ii * SequenceDelay);
            }
        }

        #endregion

        #region Public methods

        private void playNextSequence()
        {
            receiver.SendMessage("PlayNextSequence");
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com