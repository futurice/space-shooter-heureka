using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Provider
{
    /// <summary>MacOS voice provider.</summary>
    public class VoiceProviderMacOS : BaseVoiceProvider
    {

        #region Variables

#if UNITY_STANDALONE_OSX || UNITY_EDITOR
        private static readonly Regex sayRegex = new Regex(@"^([^#]+?)\s*([^ ]+)\s*# (.*?)$");
#endif

        private const int defaultRate = 175;
        private const string extension = ".aiff";

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

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
                    using (Process voicesProcess = new Process())
                    {
                        voicesProcess.StartInfo.FileName = Constants.TTS_MACOS;
                        voicesProcess.StartInfo.Arguments = "-v '?'"; //TODO --v '?'
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
                                            Match match = sayRegex.Match(reply);

                                            if (match.Success)
                                            {
                                                result.Add(new Voice(match.Groups[1].ToString(), match.Groups[3].ToString(), match.Groups[2].ToString().Replace('_', '-')));
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
#endif
                    cachedVoices = result;
                    return result;
                }
            }
        }

        public override IEnumerator SpeakNative(WrapperNative wrapper)
        {
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
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
                    yield return null;

                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    using (Process speakProcess = new Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                      (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " \"" +
                                      wrapper.Text.Replace('"', '\'') + '"';

                        if (Constants.DEBUG)
                            UnityEngine.Debug.Log("Process argruments: " + args);

                        speakProcess.StartInfo.FileName = Constants.TTS_MACOS;
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
                        onSpeakNativeStart(wrapper);

                        do
                        {
                            yield return null;

                            //if (silence)
                            //{
                            //    speakProcess.Kill();
                            //}
                        } while (!speakProcess.HasExited);

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

                        processes.Remove(wrapper.Uid);
                        onSpeakNativeComplete(wrapper);
                    }
                }
            }
#else
            yield return null;
#endif
        }

        public override IEnumerator Speak(Wrapper wrapper)
        {
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WEBPLAYER
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

                        string speaker = string.Empty;
                        int calculatedRate = calculateRate(wrapper.Rate);

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                        }
                        else
                        {
                            speaker = wrapper.Voice.Name;
                        }

                        string outputFile = Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                        using (Process speakToFileProcess = new Process())
                        {

                            string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                          (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " -o \"" +
                                          outputFile.Replace('"', '\'') + '"' +
                                          " --file-format=AIFFLE" + " \"" +
                                          wrapper.Text.Replace('"', '\'') + '"';

                            if (Constants.DEBUG)
                                UnityEngine.Debug.Log("Process argruments: " + args);

                            speakToFileProcess.StartInfo.FileName = Constants.TTS_MACOS;
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
                                    AudioClip ac = www.GetAudioClip(false, false, AudioType.AIFF);

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
                                            } while (!silence &&
                                                   (wrapper.Source != null &&
                                                   ((!wrapper.Source.loop && wrapper.Source.timeSamples > 0 && (wrapper.Source.clip != null && wrapper.Source.timeSamples < wrapper.Source.clip.samples - 4096)) ||
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

        private int calculateRate(float rate)
        {
            return Mathf.Clamp(rate != 1f ? (int)(defaultRate * rate) : defaultRate, 1, 3 * defaultRate);
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
                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    string outputFile = Constants.AUDIOFILE_PATH + wrapper.Uid + extension;

                    using (Process speakToFileProcess = new Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                      (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " -o \"" +
                                      outputFile.Replace('"', '\'') + '"' +
                                      " --file-format=AIFFLE" + " \"" +
                                      wrapper.Text.Replace('"', '\'') + '"';

                        if (Constants.DEBUG)
                            UnityEngine.Debug.Log("Process argruments: " + args);

                        speakToFileProcess.StartInfo.FileName = Constants.TTS_MACOS;
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
                    UnityEngine.Debug.LogWarning("'Text' is null or empty: " + wrapper);
                }
                else
                {
                    string speaker = string.Empty;
                    int calculatedRate = calculateRate(wrapper.Rate);

                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        UnityEngine.Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        speaker = wrapper.Voice.Name;
                    }

                    using (Process speakProcess = new Process())
                    {

                        string args = (string.IsNullOrEmpty(speaker) ? string.Empty : (" -v \"" + speaker.Replace('"', '\'') + '"')) +
                                      (calculatedRate != defaultRate ? (" -r " + calculatedRate) : string.Empty) + " \"" +
                                      wrapper.Text.Replace('"', '\'') + '"';

                        if (Constants.DEBUG)
                            UnityEngine.Debug.Log("Process argruments: " + args);

                        speakProcess.StartInfo.FileName = Constants.TTS_MACOS;
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
            }
#endif
        }

#endif

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com