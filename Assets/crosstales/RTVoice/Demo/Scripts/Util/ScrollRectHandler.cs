using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo.Util
{
    /// <summary>Changes the sensitivity of ScrollRects under various platforms.</summary>
    public class ScrollRectHandler : MonoBehaviour
    {

        #region Variables

        public ScrollRect Scroll;
        private float WindowsSensitivity = 35f;
        private float MacSensitivity = 25f;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            if (Helper.isWindowsPlatform)
            {
                Scroll.scrollSensitivity = WindowsSensitivity;
            }
            else if (Helper.isMacOSPlatform)
            {
                Scroll.scrollSensitivity = MacSensitivity;
            }
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com