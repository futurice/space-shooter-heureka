using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Demo
{
    public class NativeDisabler : MonoBehaviour
    {

        public GameObject[] Objects;

        void Update()
        {
            foreach (GameObject go in Objects)
            {
                go.SetActive(!GUISpeech.isNative);
            }
        }
    }
}