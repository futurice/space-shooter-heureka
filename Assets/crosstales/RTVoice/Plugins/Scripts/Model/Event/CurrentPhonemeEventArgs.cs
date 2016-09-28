namespace Crosstales.RTVoice.Model.Event
{
   /// <summary>EventArgs for the current phoneme.</summary>
   public class CurrentPhonemeEventArgs : SpeakNativeEventArgs {
      /// <summary>Current phoneme.</summary>
      public string Phoneme;

      public CurrentPhonemeEventArgs(WrapperNative wrapper, string phoneme) : base(wrapper) {
         Phoneme = phoneme;
      }
   }
}
// Copyright 2016 www.crosstales.com