using System;
using System.Text;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs and base class for all speaker (native) events.</summary>
    public class SpeakNativeEventArgs : EventArgs
    {
        #region Variables

        /// <summary>Wrapper with "SpeakNative"-function call.</summary>
        public WrapperNative Wrapper;

        #endregion

        #region Constructor

        public SpeakNativeEventArgs(WrapperNative wrapper)
        {
            Wrapper = wrapper;
        }

        #endregion

        #region Overridden methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(GetType().Name);
            result.Append(Constants.TEXT_TOSTRING_START);

            result.Append("Wrapper='");
            result.Append(Wrapper);
            result.Append(Constants.TEXT_TOSTRING_DELIMITER_END);

            result.Append(Constants.TEXT_TOSTRING_END);

            return result.ToString();
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com