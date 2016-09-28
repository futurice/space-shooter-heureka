namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs for the current viseme.</summary>
    public class CurrentVisemeEventArgs : SpeakNativeEventArgs
    {
        /// <summary>Current viseme.</summary>
        public string Viseme;

        public CurrentVisemeEventArgs(WrapperNative wrapper, string viseme) : base(wrapper)
        {
            Viseme = viseme;
        }
    }
}
// Copyright 2016 www.crosstales.com