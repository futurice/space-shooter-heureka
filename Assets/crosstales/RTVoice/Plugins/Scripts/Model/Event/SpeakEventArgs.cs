using System;
using System.Text;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs and base class for all speaker events.</summary>
    public class SpeakEventArgs : EventArgs
    {
        #region Variables

        /// <summary>Wrapper with "Speak"-function call.</summary>
        public Wrapper Wrapper;

        #endregion

        #region Constructor

        public SpeakEventArgs(Wrapper wrapper)
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