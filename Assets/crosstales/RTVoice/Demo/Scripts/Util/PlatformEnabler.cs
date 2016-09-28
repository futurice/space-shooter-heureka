using UnityEngine;
using Crosstales.RTVoice.Util;
using System.Collections.Generic;

namespace Crosstales.RTVoice.Demo.Util
{
    /// <summary>Enables game objects for a given platform.</summary>
    public class PlatformEnabler : MonoBehaviour
    {

        #region Variables

        public List<Platform> EnabledPlatforms;
        public GameObject[] Objects;

        private Platform currentPlatform;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            if (Helper.isWindowsPlatform)
            {
                currentPlatform = Platform.Windows;
            }
            else if (Helper.isMacOSPlatform)
            {
                currentPlatform = Platform.OSX;
            }
            else if (Helper.isAndroidPlatform)
            {
                currentPlatform = Platform.Android;
            }
            else if (Helper.isIOSPlatform)
            {
                currentPlatform = Platform.IOS;
            }
            else
            {
                currentPlatform = Platform.Unsupported;
            }
        }

        void Update()
        {

            foreach (GameObject go in Objects)
            {
                go.SetActive(EnabledPlatforms.Contains(currentPlatform));
            }
        }
    }

    #endregion

    #region Enumeration

    /// <summary>All available platforms.</summary>
    public enum Platform
    {
        OSX,
        Windows,
        IOS,
        Android,
        Unsupported
    }
    #endregion
}
// Copyright 2016 www.crosstales.com