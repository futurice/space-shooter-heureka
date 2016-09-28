using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Provider
{
    /// <summary>Windows voice provider.</summary>
    public class VoiceProviderWindows : BaseVoiceProvider
    {

        #region Variables

        private const string extension = ".wav";

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && !UNITY_WEBPLAYER
        private const string idVoice = "@VOICE:";
        private const string idSpeak = "@SPEAK";
        private const string idWord = "@WORD";
        private const string idPhoneme = "@PHONEME:";
        private const string idViseme = "@VISEME:";
        private const string idStart = "@STARTED";

        private static char[] splitChar = new char[] { ':' };
#endif

        #endregion

        #region Implemented methods

        public override string AudioFileExtension
        {
            get
            {
                return extension;
            }
        }

        public override List<Voice> Voices
        {
            get
            {
                if (cachedVoices != null)
                {
                    return cachedVoices;
                }
                else
                {
                    List<Voice> result = new List<Voice>();
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && !UNITY_WEBPLAYER
                    string application = applicationName();

                    if (File.Exists(application))
                    {
                        using (Process voicesProcess = new Process())
                        {
                            voicesProcess.StartInfo.FileName = application;
                            voicesProcess.StartInfo.Arguments = "--voices";
                            voicesProcess.StartInfo.CreateNoWindow = true;
                            voicesProcess.StartInfo.RedirectStandardOutput = true;
                            voicesProcess.StartInfo.RedirectStandardError = true;
                            voicesProcess.StartInfo.UseShellExecute = false;
                            voicesProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                            //* Set your output and error (asynchronous) handlers
                            //voicesProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                            //voicesProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                            try
                            {
                                voicesProcess.Start();
                                //voicesProcess.BeginOutputReadLine();
                                //voicesProcess.BeginErrorReadLine();

                                voicesProcess.WaitForExit(Constants.TTS_KILL_TIME);

                                if (voicesProcess.ExitCode == 0)
                                {
                                    using (StreamReader streamReader = voicesProcess.StandardOutput)
                                    {
                                        string reply;
                                        while (!streamReader.EndOfStream)
                                        {
                                            reply = streamReader.ReadLine();

                                            if (!string.IsNullOrEmpty(reply))
                                            {
                                                if (reply.StartsWith(idVoice))
                                                {

                                                    string[] splittedString = reply.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                                                    if (splittedString.Length == 6)
                                                    {
                                                        result.Add(new Voice(splittedString[1], splittedString[2], splittedString[3], splittedString[4], splittedString[5]));
                                                    }
                                                    else
                                                    {
                                                        UnityEngine.Debug.LogWarning("Voice is invalid: " + reply);
                                                    }
                                                    //                     } else if(reply.Equals("@DONE") || reply.Equals("@COMPLETED")) {
                                                    //                        complete = true;
                                                }
                                            }
                                        }
                                    }
                                    if (Constants.DEBUG)
                                        UnityEngine.Debug.Log("Voices read: " + result.CTDump());
                                }
                                else
                                {
                                    using (StreamReader sr = voicesProcess.StandardError)
                                    {
                                        string errorMessage = "Could not get any voices: " + voicesProcess.ExitCode + Environment.NewLine + sr.ReadToEnd();
                                        UnityEngine.Debug.LogError(errorMessage);
                                        onErrorInfo(errorMessage);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                string errorMessage = "Could not get any voices!" + Environment.NewLine + ex;
                                UnityEngine.Debug.LogError(errorMessage);
                                onErrorInfo(errorMessage);
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = "Could not find the TTS-wrapper: '" + application + "'";
                        UnityEngine.Debug.LogError(errorMessage);
                        onErrorInfo(errorMessage);
                    }
#endif
                    cachedVoices = result;
                    return result;
                }
            }
        }

        public override IEnumerator SpeakNative(WrapperNative wrapper)
        {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty!");
                    yield return null;
                }
                else
                {
                    yield return null;

                    string application = applicationName();

                    if (File.Exists(application))
                    {
                        int calculatedRate = calculateRate(wrapper.Rate);
                        int calculatedVolume = calculateVolume(wrapper.Volume);

                        string voiceName = string.Empty;

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                        }
                        else
                        {
                            voiceName = wrapper.Voice.Name;
                        }

                        using (Process speakProcess = new Process())
                        {

                            string args = "--speak " + '"' +
                                          wrapper.Text.Replace('"', '\'') + "\" " +
                                          calculatedRate.ToString() + " " +
                                          calculatedVolume.ToString() + " \"" +
                                          voiceName.Replace('"', '\'') + '"';

                            if (Constants.DEBUG)
                                UnityEngine.Debug.Log("Process argruments: " + args);

                            speakProcess.StartInfo.FileName = application;
                            speakProcess.StartInfo.Arguments = args;
                            speakProcess.StartInfo.CreateNoWindow = true;
                            speakProcess.StartInfo.RedirectStandardOutput = true;
                            speakProcess.StartInfo.RedirectStandardError = true;
                            speakProcess.StartInfo.UseShellExecute = false;
                            speakProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                            //* Set your output and error (asynchronous) handlers
                            //speakProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                            //scanProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                            speakProcess.Start();
                            //speakProcess.BeginOutputReadLine();
                            //speakProcess.BeginErrorReadLine();

                            silence = false;
                            processes.Add(wrapper.Uid, speakProcess);

                            string[] speechTextArray = Helper.CleanText(wrapper.Text, false).Split(splitCharWords, StringSplitOptions.RemoveEmptyEntries);
                            int wordIndex = 0;
                            int wordIndexCompare = 0;
                            string phoneme = string.Empty;
                            string viseme = string.Empty;
                            bool start = false;
                            //bool complete = false;

                            Thread worker = new Thread(() => readSpeakNativeStream(speakProcess, speechTextArray, out wordIndex, out phoneme, out viseme, out start)) { Name = wrapper.Uid.ToString() };
                            worker.Start();

                            do
                            {
                                yield return null;

                                if (wordIndex != wordIndexCompare)
                                {
                                    onSpeakNativeCurrentWord(wrapper, speechTextArray, wordIndex - 1);

                                    wordIndexCompare = wordIndex;
                                }

                                if (!string.IsNullOrEmpty(phoneme))
                                {
                                    onSpeakNativeCurrentPhoneme(wrapper, phoneme);

                                    phoneme = string.Empty;
                                }

                                if (!string.IsNullOrEmpty(viseme))
                                {
                                    onSpeakNativeCurrentViseme(wrapper, viseme);

                                    viseme = string.Empty;
                                }

                                if (start)
                                {
                                    onSpeakNativeStart(wrapper);

                                    start = false;
                                }
                                //
                                //                  if (complete) {
                                //                     SpeakNativeCompleteMethod(uid, speechText, voice, rate, volume);
                                //
                                //                     complete = false;
                                //                  }

                                //if (silence)
                                //{
                                //    speakProcess.Kill();
                                //}
                            } while (!speakProcess.HasExited);

                            // clear output
                            onSpeakNativeCurrentPhoneme(wrapper, string.Empty);
                            onSpeakNativeCurrentViseme(wrapper, string.Empty);

                            if (speakProcess.ExitCode == 0 || speakProcess.ExitCode == -1) //0 = normal ended, -1 = killed
                            {
                                if (Constants.DEBUG)
                                    UnityEngine.Debug.Log("Text spoken: " + wrapper.Text);
                            }
                            else
                            {
                                using (StreamReader sr = speakProcess.StandardError)
                                {
                                    string errorMessage = "Could not speak the text: " + speakProcess.ExitCode + Environment.NewLine + sr.ReadToEnd();
                                    UnityEngine.Debug.LogError(errorMessage);
                                    onErrorInfo(errorMessage);
                                }
                            }

                            onSpeakNativeComplete(wrapper);
                            processes.Remove(wrapper.Uid);
                        }
                    }
                    else
                    {
                        string errorMessage = "Could not find the TTS-wrapper: '" + application + "'";
                        UnityEngine.Debug.LogError(errorMessage);
                        onErrorInfo(errorMessage);
                    }
                }
            }
#else
            yield return null;
#endif
        }

        public override IEnumerator Speak(Wrapper wrapper)
        {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty: " + wrapper);
                    yield return null;
                }
                else
                {
                    if (wrapper.Source == null)
                    {
                        UnityEngine.Debug.LogWarning("'Source' is null: " + wrapper);
                        yield return null;
                    }
                    else
                    {
                        yield return null;

                        string application = applicationName();

                        if (File.Exists(application))
                        {
                            int calculatedRate = calculateRate(wrapper.Rate);
                            int calculatedVolume = calculateVolume(wrapper.Volume);

                            string voiceName = string.Empty;

                            if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                            {
                                UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                            }
                            else
                            {
                                voiceName = wrapper.Voice.Name;
                            }

                            using (Process speakToFileProcess = new Process())
                            {
                                string outputFile = Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                                string args = "--speakToFile" + " \"" +
                                              wrapper.Text.Replace('"', '\'') + "\" \"" +
                                              outputFile.Replace('"', '\'') + "\" " +
                                              calculatedRate.ToString() + " " +
                                              calculatedVolume.ToString() + " \"" +
                                              voiceName.Replace('"', '\'') + '"';

                                if (Constants.DEBUG)
                                    UnityEngine.Debug.Log("Process argruments: " + args);

                                speakToFileProcess.StartInfo.FileName = application;
                                speakToFileProcess.StartInfo.Arguments = args;
                                speakToFileProcess.StartInfo.CreateNoWindow = true;
                                speakToFileProcess.StartInfo.RedirectStandardOutput = true;
                                speakToFileProcess.StartInfo.RedirectStandardError = true;
                                speakToFileProcess.StartInfo.UseShellExecute = false;
                                speakToFileProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                                //* Set your output and error (asynchronous) handlers
                                //speakToFileProcess.OutputDataReceived += new DataReceivedEventHandler(speakNativeHandler);
                                //speakToFileProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                                speakToFileProcess.Start();
                                //speakToFileProcess.BeginOutputReadLine();
                                //speakToFileProcess.BeginErrorReadLine();

                                silence = false;
                                onSpeakAudioGenerationStart(wrapper);

                                do
                                {
                                    yield return null;
                                } while (!speakToFileProcess.HasExited);

                                if (speakToFileProcess.ExitCode == 0)
                                {
                                    WWW www = new WWW("file:///" + outputFile);

                                    if (string.IsNullOrEmpty(www.error))
                                    {
                                        AudioClip ac = www.GetAudioClip(false, false, AudioType.WAV);
                                        //AudioClip ac = www.GetAudioClipCompressed(false, AudioType.WAV);

                                        do
                                        {
                                            yield return www;
                                        } while (ac.loadState == AudioDataLoadState.Loading);


                                        if (wrapper.Source != null && ac.loadState == AudioDataLoadState.Loaded)
                                        {
                                            wrapper.Source.clip = ac;

                                            //yield return null;

                                            if (!string.IsNullOrEmpty(wrapper.OutputFile))
                                            {
                                                fileCopy(outputFile, wrapper.OutputFile, Constants.AUDIOFILE_AUTOMATIC_DELETE);
                                            }

                                            if (Constants.AUDIOFILE_AUTOMATIC_DELETE)
                                            {
                                                if (File.Exists(outputFile))
                                                {
                                                    try
                                                    {
                                                        File.Delete(outputFile);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        string errorMessage = "Could not delete file '" + outputFile + "'!" + Environment.NewLine + ex;
                                                        UnityEngine.Debug.LogError(errorMessage);
                                                        onErrorInfo(errorMessage);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(wrapper.OutputFile))
                                                {
                                                    wrapper.OutputFile = outputFile;
                                                }
                                            }

                                            if (Constants.DEBUG)
                                                UnityEngine.Debug.Log("Text generated: " + wrapper.Text);

                                            if (wrapper.SpeakImmediately && wrapper.Source != null)
                                            {
                                                wrapper.Source.Play();

                                                onSpeakStart(wrapper);

                                                do
                                                {
                                                    yield return null;
                                                } while (!silence && (wrapper.Source != null &&
                                                            ((!wrapper.Source.loop && wrapper.Source.timeSamples > 0 &&
                                                            (wrapper.Source.clip != null && wrapper.Source.timeSamples < wrapper.Source.clip.samples - 4096)) ||
                                                            wrapper.Source.isPlaying)));

                                                if (Constants.DEBUG)
                                                    UnityEngine.Debug.Log("Text spoken: " + wrapper.Text);

                                                onSpeakComplete(wrapper);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string errorMessage = "Could not read the file: " + www.error;
                                        UnityEngine.Debug.LogError(errorMessage);
                                        onErrorInfo(errorMessage);
                                    }

                                    www.Dispose();
                                }
                                else
                                {
                                    using (StreamReader sr = speakToFileProcess.StandardError)
                                    {
                                        string errorMessage = "Could not speak the text: " + speakToFileProcess.ExitCode + Environment.NewLine + sr.ReadToEnd();
                                        UnityEngine.Debug.LogError(errorMessage);
                                        onErrorInfo(errorMessage);
                                    }
                                }

                                onSpeakAudioGenerationComplete(wrapper);

                                //UnityEngine.Debug.Log("Speak complete: " + wrapper.Text);
                            }
                        }
                        else
                        {
                            string errorMessage = "Could not find the TTS-wrapper: '" + application + "'";
                            UnityEngine.Debug.LogError(errorMessage);
                            onErrorInfo(errorMessage);
                        }
                    }
                }
            }
#else
         yield return null;
#endif
        }

        #endregion

        #region Private methods

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && !UNITY_WEBPLAYER
        private void readSpeakNativeStream(Process process, string[] speechTextArray, out int wordIndex, out string phoneme, out string viseme, out bool start)
        {
            string reply;

            wordIndex = 0;
            phoneme = string.Empty;
            viseme = string.Empty;
            start = false;
            //complete = false;

            using (StreamReader streamReader = process.StandardOutput)
            {
                reply = streamReader.ReadLine();
                if (reply.Equals(idSpeak))
                {

                    while (!process.HasExited)
                    {
                        reply = streamReader.ReadLine();

                        if (!string.IsNullOrEmpty(reply))
                        {
                            if (reply.StartsWith(idWord))
                            {
                                if (speechTextArray[wordIndex].Equals("-"))
                                {
                                    wordIndex++;
                                }

                                wordIndex++;
                            }
                            else if (reply.StartsWith(idPhoneme))
                            {

                                string[] splittedString = reply.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                                if (splittedString.Length > 1)
                                {
                                    phoneme = splittedString[1];
                                    //}
                                    //else
                                    //{
                                    //    UnityEngine.Debug.LogWarning("Phoneme was empty!");
                                }
                            }
                            else if (reply.StartsWith(idViseme))
                            {

                                string[] splittedString = reply.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                                if (splittedString.Length > 1)
                                {
                                    viseme = splittedString[1];
                                    //}
                                    //else
                                    //{
                                    //    UnityEngine.Debug.LogWarning("Viseme was empty!");
                                }
                            }
                            else if (reply.Equals(idStart))
                            {
                                start = true;
                                //                     } else if (reply.Equals("@DONE") || reply.Equals("@COMPLETED")) {
                                //                        complete = true;
                            }
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Unexpected process output: " + reply + Environment.NewLine + streamReader.ReadToEnd());
                }
            }
        }
#endif

        private string applicationName()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Constants.ENFORCE_32BIT_WINDOWS)
                {
                    return Constants.TTS_WINDOWS_EDITOR_x86;
                }
                else
                {
                    return Constants.TTS_WINDOWS_EDITOR;
                }
            }
            else
            {
                return Constants.TTS_WINDOWS_BUILD;
            }
        }

        private int calculateVolume(float volume)
        {
            return Mathf.Clamp((int)(100 * volume), 0, 100);
        }

        private int calculateRate(float rate)
        { //allowed range: 0 - 3f - all other values were cropped
            int result = 0;

            if (rate != 1f)
            { //relevant?
                if (rate > 1f)
                { //larger than 1
                    if (rate >= 2.75f)
                    {
                        result = 10; //2.78
                    }
                    else if (rate >= 2.6f && rate < 2.75f)
                    {
                        result = 9; //2.6
                    }
                    else if (rate >= 2.35f && rate < 2.6f)
                    {
                        result = 8; //2.39
                    }
                    else if (rate >= 2.2f && rate < 2.35f)
                    {
                        result = 7; //2.2
                    }
                    else if (rate >= 2f && rate < 2.2f)
                    {
                        result = 6; //2
                    }
                    else if (rate >= 1.8f && rate < 2f)
                    {
                        result = 5; //1.8
                    }
                    else if (rate >= 1.6f && rate < 1.8f)
                    {
                        result = 4; //1.6
                    }
                    else if (rate >= 1.4f && rate < 1.6f)
                    {
                        result = 3; //1.45
                    }
                    else if (rate >= 1.2f && rate < 1.4f)
                    {
                        result = 2; //1.28
                    }
                    else if (rate > 1f && rate < 1.2f)
                    {
                        result = 1; //1.14
                    }
                }
                else
                { //smaller than 1
                    if (rate <= 0.3f)
                    {
                        result = -10; //0.33
                    }
                    else if (rate > 0.3 && rate <= 0.4f)
                    {
                        result = -9; //0.375
                    }
                    else if (rate > 0.4 && rate <= 0.45f)
                    {
                        result = -8; //0.42
                    }
                    else if (rate > 0.45 && rate <= 0.5f)
                    {
                        result = -7; //0.47
                    }
                    else if (rate > 0.5 && rate <= 0.55f)
                    {
                        result = -6; //0.525
                    }
                    else if (rate > 0.55 && rate <= 0.6f)
                    {
                        result = -5; //0.585
                    }
                    else if (rate > 0.6 && rate <= 0.7f)
                    {
                        result = -4; //0.655
                    }
                    else if (rate > 0.7 && rate <= 0.8f)
                    {
                        result = -3; //0.732
                    }
                    else if (rate > 0.8 && rate <= 0.9f)
                    {
                        result = -2; //0.82
                    }
                    else if (rate > 0.9 && rate < 1f)
                    {
                        result = -1; //0.92
                    }
                }
            }

            //UnityEngine.Debug.Log(result + " - " + rate);

            return result;
        }

        #endregion

        #region Editor-only methods

#if UNITY_EDITOR

        public override void GenerateInEditor(Wrapper wrapper)
        {
#if !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty: " + wrapper);
                }
                else
                {
                    string application = applicationName();

                    if (File.Exists(application))
                    {
                        int calculatedRate = calculateRate(wrapper.Rate);
                        int calculatedVolume = calculateVolume(wrapper.Volume);

                        string voiceName = string.Empty;

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                        }
                        else
                        {
                            voiceName = wrapper.Voice.Name;
                        }

                        using (Process speakToFileProcess = new Process())
                        {
                            string outputFile = Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                            string args = "--speakToFile" + " \"" +
                                          wrapper.Text.Replace('"', '\'') + "\" \"" +
                                          outputFile.Replace('"', '\'') + "\" " +
                                          calculatedRate.ToString() + " " +
                                          calculatedVolume.ToString() + " \"" +
                                          voiceName.Replace('"', '\'') + '"';

                            if (Constants.DEBUG)
                                UnityEngine.Debug.Log("Process argruments: " + args);

                            speakToFileProcess.StartInfo.FileName = application;
                            speakToFileProcess.StartInfo.Arguments = args;
                            speakToFileProcess.StartInfo.CreateNoWindow = true;
                            speakToFileProcess.StartInfo.RedirectStandardOutput = true;
                            speakToFileProcess.StartInfo.RedirectStandardError = true;
                            speakToFileProcess.StartInfo.UseShellExecute = false;
                            speakToFileProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                            speakToFileProcess.Start();

                            silence = false;

                            while (!speakToFileProcess.HasExited)
                            {
                                Thread.Sleep(20);
                            }

                            if (speakToFileProcess.ExitCode == 0)
                            {

                                if (!string.IsNullOrEmpty(wrapper.OutputFile))
                                {
                                    fileCopy(outputFile, wrapper.OutputFile, Constants.AUDIOFILE_AUTOMATIC_DELETE);
                                }

                                if (Constants.AUDIOFILE_AUTOMATIC_DELETE)
                                {
                                    if (File.Exists(outputFile))
                                    {
                                        try
                                        {
                                            File.Delete(outputFile);
                                        }
                                        catch (Exception ex)
                                        {
                                            UnityEngine.Debug.LogError("Could not delete file '" + outputFile + "'!" + Environment.NewLine + ex);
                                        }
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(wrapper.OutputFile))
                                    {
                                        wrapper.OutputFile = outputFile;
                                    }
                                }

                                if (Constants.DEBUG)
                                    UnityEngine.Debug.Log("Text generated: " + wrapper.Text);
                            }
                            else
                            {
                                using (StreamReader sr = speakToFileProcess.StandardError)
                                {
                                    UnityEngine.Debug.LogError("Could not speak the text: " + speakToFileProcess.ExitCode + Environment.NewLine + sr.ReadToEnd());
                                }
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Could not find the TTS-wrapper: '" + application + "'");
                    }
                }
            }
#endif
        }

        public override void SpeakNativeInEditor(WrapperNative wrapper)
        {
#if !UNITY_WEBPLAYER
            if (wrapper == null)
            {
                UnityEngine.Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    UnityEngine.Debug.LogWarning("'Text' is null or empty!");
                }
                else
                {

                    string application = applicationName();

                    if (File.Exists(application))
                    {
                        int calculatedRate = calculateRate(wrapper.Rate);
                        int calculatedVolume = calculateVolume(wrapper.Volume);

                        string voiceName = string.Empty;

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                        }
                        else
                        {
                            voiceName = wrapper.Voice.Name;
                        }

                        using (Process speakProcess = new Process())
                        {

                            string args = "--speak " + '"' +
                                          wrapper.Text.Replace('"', '\'') + "\" " +
                                          calculatedRate.ToString() + " " +
                                          calculatedVolume.ToString() + " \"" +
                                          voiceName.Replace('"', '\'') + '"';

                            if (Constants.DEBUG)
                                UnityEngine.Debug.Log("Process argruments: " + args);

                            speakProcess.StartInfo.FileName = application;
                            speakProcess.StartInfo.Arguments = args;
                            speakProcess.StartInfo.CreateNoWindow = true;
                            speakProcess.StartInfo.RedirectStandardOutput = true;
                            speakProcess.StartInfo.RedirectStandardError = true;
                            speakProcess.StartInfo.UseShellExecute = false;
                            speakProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                            speakProcess.Start();
                            silence = false;

                            while (!speakProcess.HasExited)
                            {
                                Thread.Sleep(10);

                                if (silence)
                                {
                                    speakProcess.Kill();
                                }
                            }

                            if (speakProcess.ExitCode == 0 || speakProcess.ExitCode == -1) //0 = normal ended, -1 = killed
                            {
                                if (Constants.DEBUG)
                                    UnityEngine.Debug.Log("Text spoken: " + wrapper.Text);
                            }
                            else
                            {
                                using (StreamReader sr = speakProcess.StandardError)
                                {
                                    UnityEngine.Debug.LogError("Could not speak the text: " + speakProcess.ExitCode + Environment.NewLine + sr.ReadToEnd());
                                }
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Could not find the TTS-wrapper: '" + application + "'");
                    }
                }
            }
#endif
        }

#endif

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com