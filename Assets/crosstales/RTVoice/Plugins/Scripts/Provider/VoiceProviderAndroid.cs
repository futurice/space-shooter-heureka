using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Provider
{

    /// <summary>Android voice provider.</summary>
    public class VoiceProviderAndroid : BaseVoiceProvider
    {


        #region Variables

        private const string extension = ".wav";

#if (UNITY_ANDROID)
        private static bool isInitialized = false;
        private static AndroidJavaObject TtsHandler;

        private const float waitForSeconds = 0.07f;
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

#if (UNITY_ANDROID)

                    if (!isInitialized)
                    {
                        initializeTTS();

                        float time = Time.realtimeSinceStartup;

                        do
                        {
                            // waiting...
                            //TODO find a better solution for this...
                        } while (Time.realtimeSinceStartup - time < Constants.TTS_KILL_TIME / 1000 && !(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));

                    }

                    try
                    {
                        string[] myStringVoices = TtsHandler.CallStatic<string[]>("GetVoices");

                        foreach (string voice in myStringVoices)
                        {
                            string[] currentVoiceData = voice.Split(';');
                            Voice newVoice = new Voice(currentVoiceData[0], "Android voice: " + voice, currentVoiceData[1]);
                            result.Add(newVoice);
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = "Could not get any voices!" + Environment.NewLine + ex;
                        Debug.LogError(errorMessage);
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

#if (UNITY_ANDROID)

            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty!");
                    yield return null;
                }
                else
                {
                    if (!isInitialized)
                    {
                        initializeTTS();

                        do
                        {
                            // waiting...
                            //TODO find a better solution for this...

                            yield return new WaitForSeconds(waitForSeconds);

                        } while (!(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));
                    }

                    string voiceName = string.Empty;
                    if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                    {
                        Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS 'default' voice.");
                    }
                    else
                    {
                        voiceName = wrapper.Voice.Name;
                    }
                    silence = false;
                    onSpeakNativeStart(wrapper);

                    TtsHandler.CallStatic("SpeakNative", new object[] { wrapper.Text, wrapper.Rate, wrapper.Pitch, wrapper.Volume, voiceName });

                    do
                    {
                        yield return new WaitForSeconds(waitForSeconds);
                    } while (!silence && TtsHandler.CallStatic<bool>("isWorking"));

                    if (Constants.DEBUG)
                        Debug.Log("Text spoken: " + wrapper.Text);

                    onSpeakNativeComplete(wrapper);
                }
            }

#else

            yield return null;

#endif

        }

        public override IEnumerator Speak(Wrapper wrapper)
        {

#if (UNITY_ANDROID)

            if (wrapper == null)
            {
                Debug.LogWarning("'wrapper' is null!");
            }
            else
            {
                if (string.IsNullOrEmpty(wrapper.Text))
                {
                    Debug.LogWarning("'Text' is null or empty: " + wrapper);
                    yield return null;
                }
                else
                {
                    if (wrapper.Source == null)
                    {
                        Debug.LogWarning("'Source' is null: " + wrapper);
                        yield return null;
                    }
                    else
                    {
                        if (!isInitialized)
                        {
                            initializeTTS();

                            do
                            {
                                // waiting...
                                //TODO find a better solution for this...

                                yield return new WaitForSeconds(waitForSeconds);

                            } while (!(isInitialized = TtsHandler.CallStatic<bool>("isInitalized")));
                        }

                        string voiceName = string.Empty;

                        if (wrapper.Voice == null || string.IsNullOrEmpty(wrapper.Voice.Name))
                        {
                            Debug.LogWarning("'Voice' or 'Voice.Name' is null! Using the OS's 'default' voice.");
                        }
                        else
                        {
                            voiceName = wrapper.Voice.Name;
                        }

                        string outputFile = Application.persistentDataPath + "/" + wrapper.Uid + extension;

                        TtsHandler.CallStatic<string>("Speak", new object[] { wrapper.Text, wrapper.Rate, wrapper.Pitch, voiceName, outputFile });

                        silence = false;
                        onSpeakAudioGenerationStart(wrapper);

                        do
                        {
                            yield return new WaitForSeconds(waitForSeconds);
                        } while (!silence && TtsHandler.CallStatic<bool>("isWorking"));

                        WWW www = new WWW("file://" + outputFile);
                        
                        if (string.IsNullOrEmpty(www.error))
                        {
                            AudioClip ac = www.GetAudioClip(false, false, AudioType.WAV); //TODO streaming to true?
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
                                            Debug.LogError(errorMessage);
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
                                        Debug.Log("Text spoken: " + wrapper.Text);

                                    wrapper.Source.Stop();
                                    wrapper.Source.clip = null;

                                    onSpeakComplete(wrapper);
                                }
                            }
                        }
                        else
                        {
                            string errorMessage = "Could not read the file: " + www.error;
                            Debug.LogError(errorMessage);
                            onErrorInfo(errorMessage);
                        }

                        www.Dispose();

                        onSpeakAudioGenerationComplete(wrapper);
                    }
                }
            }

#else

            yield return null;

#endif

        }

        public override void Silence()
        {
            silence = true;

#if (UNITY_ANDROID)
            TtsHandler.CallStatic("StopNative");
#endif
        }


        #endregion



        #region Public methods


        public void ShutdownTTS()
        {
#if (UNITY_ANDROID)
            TtsHandler.CallStatic("Shutdown");
#endif
        }


        #endregion



        #region Private methods


#if (UNITY_ANDROID)

        private void initializeTTS()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            TtsHandler = new AndroidJavaObject("com.crosstales.RTVoice.RTVoiceAndroidBridge", new object[] { jo });
        }

#endif

        #endregion


        #region Editor-only methods


#if UNITY_EDITOR

        public override void GenerateInEditor(Wrapper wrapper)
        {
            Debug.LogError("GenerateInEditor is not supported for Unity Android!");
        }

        public override void SpeakNativeInEditor(WrapperNative wrapper)
        {
            Debug.LogError("SpeakNativeInEditor is not supported for Unity Android!");
        }

#endif


        #endregion



    }
}