using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Model.Event;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Provider
{
    /// <summary>Base class for voice providers.</summary>
    public abstract class BaseVoiceProvider
    {

        #region Variables

        protected static List<Voice> cachedVoices;

        protected Dictionary<Guid, Process> processes = new Dictionary<Guid, Process>();

        protected bool silence = false;

        protected static char[] splitCharWords = new char[] { ' ' };
        
        #endregion

        #region Properties

        /// <summary>Returns the extension of the generated audio files.</summary>
        /// <returns>Extension of the generated audio files.</returns>
        public abstract string AudioFileExtension
        {
            get;
        }

        /// <summary>Get all available voices from the current TTS-provider and fills it into a given list.</summary>
        /// <returns>All available voices from the current TTS-provider as list.</returns>
        public abstract List<Voice> Voices
        {
            get;
        }

        #endregion

        #region Events

        public delegate void SpeakNativeCurrentWord(CurrentWordEventArgs e);
        public delegate void SpeakNativeCurrentPhoneme(CurrentPhonemeEventArgs e);
        public delegate void SpeakNativeCurrentViseme(CurrentVisemeEventArgs e);
        public delegate void SpeakNativeStart(SpeakNativeEventArgs e);
        public delegate void SpeakNativeComplete(SpeakNativeEventArgs e);
        public delegate void SpeakStart(SpeakEventArgs e);
        public delegate void SpeakComplete(SpeakEventArgs e);
        public delegate void SpeakAudioGenerationStart(SpeakEventArgs e);
        public delegate void SpeakAudioGenerationComplete(SpeakEventArgs e);
        public delegate void ErrorInfo(string info);

        /// <summary>An event triggered whenever a new word is spoken (native mode, Windows only).</summary>
        public static event SpeakNativeCurrentWord OnSpeakNativeCurrentWord;

        /// <summary>An event triggered whenever a new phoneme is spoken (native mode, Windows only).</summary>
        public static event SpeakNativeCurrentPhoneme OnSpeakNativeCurrentPhoneme;

        /// <summary>An event triggered whenever a new viseme is spoken  (native mode, Windows only).</summary>
        public static event SpeakNativeCurrentViseme OnSpeakNativeCurrentViseme;

        /// <summary>An event triggered whenever a native speak is started.</summary>
        public static event SpeakNativeStart OnSpeakNativeStart;

        /// <summary>An event triggered whenever a native speak is completed.</summary>
        public static event SpeakNativeComplete OnSpeakNativeComplete;

        /// <summary>An event triggered whenever a speak is started.</summary>
        public static event SpeakStart OnSpeakStart;

        /// <summary>An event triggered whenever a native speak is completed.</summary>
        public static event SpeakComplete OnSpeakComplete;

        /// <summary>An event triggered whenever a speak audio generation is started.</summary>
        public static event SpeakAudioGenerationStart OnSpeakAudioGenerationStart;

        /// <summary>An event triggered whenever a speak audio generation is completed.</summary>
        public static event SpeakAudioGenerationComplete OnSpeakAudioGenerationComplete;

        /// <summary>An event triggered whenever an error occurs.</summary>
        public static event ErrorInfo OnErrorInfo;

        #endregion

        #region Public methods

        /// <summary>Silence all active TTS-providers.</summary>
        public virtual void Silence()
        {
            silence = true;

#if UNITY_STANDALONE || (UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX))

         foreach (KeyValuePair<Guid, Process> kvp in processes) {
            if (!kvp.Value.HasExited) {
               kvp.Value.Kill();
            }
         }

#endif

            processes.Clear();
        }

        /// <summary>Silence the current TTS-provider (native mode).</summary>
        /// <param name="uid">UID of the speaker</param>
        public virtual void Silence(Guid uid)
        {
#if UNITY_STANDALONE || (UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX))
         if (processes.ContainsKey(uid) && !processes [uid].HasExited) {
            processes [uid].Kill();
         }

#endif
            processes.Remove(uid);
        }

        /// <summary>The current provider speaks a text with a given voice (native mode).</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract IEnumerator SpeakNative(WrapperNative wrapper);

        /// <summary>The current provider speaks a text with a given voice.</summary>
        /// <param name="wrapper">Wrapper containing the data.</param>
        public abstract IEnumerator Speak(Wrapper wrapper);

        #endregion

        #region Protected methods

        protected void fileCopy(string inputFile, string outputFile, bool move = false)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
         if(!string.IsNullOrEmpty(outputFile)) {
            try {
               if (!File.Exists(inputFile)) {
                  UnityEngine.Debug.LogError("Input file does not exists: " + inputFile);
               } else {

                  if (File.Exists(outputFile)) {
                     //UnityEngine.Debug.LogWarning("Overwrite output file: " + outputFile);
                     File.Delete(outputFile);
                  }

                  if (move) {
#if UNITY_STANDALONE || (UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX))
                     File.Move(inputFile, outputFile);
#else
                     File.Copy(inputFile, outputFile);
                     File.Delete(inputFile);
#endif
                  } else {
                     File.Copy(inputFile, outputFile);
                  }
               }
            } catch (DirectoryNotFoundException ex) {
               UnityEngine.Debug.LogError("Destination directory '" + outputFile + "' not found!" + Environment.NewLine + ex); 
//            } catch (IOException ex) {
//               UnityEngine.Debug.LogError("IOException - Could not copy file!" + Environment.NewLine + ex);
            } catch (Exception ex) {
               UnityEngine.Debug.LogError("Could not copy file!" + Environment.NewLine + ex); 
            }
         }
#endif
        }

        protected static void onSpeakNativeCurrentWord(WrapperNative wrapper, string[] speechTextArray, int wordIndex)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak current word: " + speechTextArray[wordIndex] + Environment.NewLine + wrapper);

            if (OnSpeakNativeCurrentWord != null)
            {
                OnSpeakNativeCurrentWord(new CurrentWordEventArgs(wrapper, speechTextArray, wordIndex));
            }
        }

        protected static void onSpeakNativeCurrentPhoneme(WrapperNative wrapper, string phoneme)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak current phoneme: " + phoneme + Environment.NewLine + wrapper);

            if (OnSpeakNativeCurrentPhoneme != null)
            {
                OnSpeakNativeCurrentPhoneme(new CurrentPhonemeEventArgs(wrapper, phoneme));
            }
        }

        protected static void onSpeakNativeCurrentViseme(WrapperNative wrapper, string viseme)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak current viseme: " + viseme + Environment.NewLine + wrapper);

            if (OnSpeakNativeCurrentViseme != null)
            {
                OnSpeakNativeCurrentViseme(new CurrentVisemeEventArgs(wrapper, viseme));
            }
        }

        protected static void onSpeakNativeStart(WrapperNative wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak native start: " + wrapper);

            if (OnSpeakNativeStart != null)
            {
                OnSpeakNativeStart(new SpeakNativeEventArgs(wrapper));
            }
        }

        protected static void onSpeakNativeComplete(WrapperNative wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak native complete: " + wrapper);

            if (OnSpeakNativeComplete != null)
            {
                OnSpeakNativeComplete(new SpeakNativeEventArgs(wrapper));
            }
        }

        protected static void onSpeakStart(Wrapper wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak start: " + wrapper);

            if (OnSpeakStart != null)
            {
                OnSpeakStart(new SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakComplete(Wrapper wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak complete: " + wrapper);

            if (OnSpeakComplete != null)
            {
                OnSpeakComplete(new SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakAudioGenerationStart(Wrapper wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak audio generation start: " + wrapper);

            if (OnSpeakAudioGenerationStart != null)
            {
                OnSpeakAudioGenerationStart(new SpeakEventArgs(wrapper));
            }
        }

        protected static void onSpeakAudioGenerationComplete(Wrapper wrapper)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Speak audio generation complete: " + wrapper);

            if (OnSpeakAudioGenerationComplete != null)
            {
                OnSpeakAudioGenerationComplete(new SpeakEventArgs(wrapper));
            }
        }

        protected static void onErrorInfo(string info)
        {
            if (Constants.DEBUG)
                UnityEngine.Debug.Log("Error info: " + info);

            if (OnErrorInfo != null)
            {
                OnErrorInfo(info);
            }
        }
        #endregion

        #region Editor-only methods

#if UNITY_EDITOR

      /// <summary>The current provider speaks a text with a given voice (native mode & Editor only).</summary>
      /// <param name="wrapper">Wrapper containing the data.</param>
      public abstract void SpeakNativeInEditor(WrapperNative wrapper);

      /// <summary>Generates audio with the current provider.</summary>
      /// <param name="wrapper">Wrapper containing the data.</param>
      public abstract void GenerateInEditor(Wrapper wrapper);

#endif

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com