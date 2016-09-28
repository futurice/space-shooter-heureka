using UnityEngine;
using System.Collections;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple "SendMessage" example.</summary>
    public class SendMessage : MonoBehaviour
    {

        #region Variables
        [Multiline]
        public string TextA = "RT-Voice works great with PlayMaker, SALSA, Localized Dialogs/Cutscenes, Dialogue System for Unity and THE Dialogue Engine - that's awesome!";
        [Multiline]
        public string TextB = "Absolutely true! RT-Voice is fantastic.";
        public float DelayTextB = 12.2f;
        public bool PlayOnStart = false;

        private GameObject receiver;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            // Find the receiver
            receiver = GameObject.Find(Constants.RTVOICE_SCENE_OBJECT_NAME);

            if (PlayOnStart)
            {
                Play();
            }
        }

        #endregion

        #region Public methods

        public void Play()
        {
            if (receiver != null)
            {
                //Speak
                SpeakerA();

                StartCoroutine(SpeakerB());
            }
            else
            {
                Debug.LogError("No gameobject 'RTVoice' found! Did you add the prefab or scripts to the scene?");
            }
        }

        public void SpeakerA()
        {
            //example with string-array
            //receiver.SendMessage("Speak", new string[]{TextA, "en", (Helper.isWindowsPlatform ? "Microsoft David Desktop" : "Alex")}); //example with string-array

            //example with string
            receiver.SendMessage("Speak", TextA + ";" + "en" + ";" + (Helper.isWindowsPlatform ? "Microsoft David Desktop" : "Alex")); //example with string-array
        }

        public IEnumerator SpeakerB()
        {
            yield return new WaitForSeconds(DelayTextB);

            //example with string-array
            receiver.SendMessage("Speak", new string[] { TextB, "en", (Helper.isWindowsPlatform ? "Microsoft Zira Desktop" : "Vicki") });

            //example with string
            //receiver.SendMessage("Speak", TextB + ";" + "en" + ";" + (Helper.isWindowsPlatform ? "Microsoft Zira Desktop" : "Vicki")); //example with string-array
        }

        public void Silence()
        {
            StopAllCoroutines();
            receiver.SendMessage("Silence");
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com