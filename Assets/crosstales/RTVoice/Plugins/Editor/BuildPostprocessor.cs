using UnityEditor;
using UnityEditor.Callbacks;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>BuildPostprocessor for Windows. Adds the TTS-wrapper to the build.</summary>
    public class BuildPostprocessor
    {

        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                string dataPath = pathToBuiltProject.Substring(0, pathToBuiltProject.Length - 4) + "_Data/";

                if (Constants.ENFORCE_32BIT_WINDOWS)
                {
                    FileUtil.CopyFileOrDirectory(Constants.TTS_WINDOWS_EDITOR_x86, dataPath + "RTVoiceTTSWrapper.exe");
                }
                else
                {
                    FileUtil.CopyFileOrDirectory(Constants.TTS_WINDOWS_EDITOR, dataPath + "RTVoiceTTSWrapper.exe");
                }

                if (Constants.DEBUG)
                    UnityEngine.Debug.Log("Wrapper copied to: " + dataPath);
            }
        }
    }
}
// Copyright 2015-2016 www.crosstales.com