namespace Crosstales.RTVoice.Model.Event
{
    /// <summary>EventArgs for the current word.</summary>
    public class CurrentWordEventArgs : SpeakNativeEventArgs
    {
        /// <summary>Array with the text splitted into words.</summary>
        public string[] SpeechTextArray;

        /// <summary>Current word index.</summary>
        public int WordIndex;

        public CurrentWordEventArgs(WrapperNative wrapper, string[] speechTextArray, int wordIndex) : base(wrapper)
        {
            SpeechTextArray = speechTextArray;
            WordIndex = wordIndex;
        }
    }
}
// Copyright 2016 www.crosstales.com