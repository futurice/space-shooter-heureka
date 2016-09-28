using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Crosstales.RTVoice.Util
{
    /// <summary>Various helper functions.</summary>
    public static class Helper
    {

        #region Variables

        private static readonly Regex lineEndingsRegex = new Regex(@"\r\n|\r|\n");
        //private static readonly Regex cleanStringRegex = new Regex(@"([^a-zA-Z0-9 ]|[ ]{2,})");
        private static readonly Regex cleanSpacesRegex = new Regex(@"\s+");

        private const string WINDOWS_PATH_DELIMITER = @"\";
        private const string UNIX_PATH_DELIMITER = "/";

        #endregion

        #region Static properties

        /// <summary>Checks if the current platform is Windows.</summary>
        /// <returns>True if the current platform is Windows.</returns>
        public static bool isWindowsPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
            }
        }

        /// <summary>Checks if the current platform is OSX.</summary>
        /// <returns>True if the current platform is OSX.</returns>
        public static bool isMacOSPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor;
            }
        }

        /// <summary>Checks if the current platform is Android.</summary>
        /// <returns>True if the current platform is Android.</returns>
        public static bool isAndroidPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.Android;
            }
        }

        /// <summary>Checks if the current platform is iOS.</summary>
        /// <returns>True if the current platform is iOS.</returns>
        public static bool isIOSPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.IPhonePlayer;
            }
        }

        /// <summary>Checks if we are in Editor mode.</summary>
        /// <returns>True if in Editor mode.</returns>
        public static bool isEditorMode
        {
            get
            {
                return (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) && !Application.isPlaying;
            }
        }

        /// <summary>Checks if the current platform is supported.</summary>
        /// <returns>True if the current platform is supported.</returns>
        public static bool isSupportedPlatform
        {
            get
            {
                return isWindowsPlatform || isMacOSPlatform || isAndroidPlatform || isIOSPlatform;
            }
        }

        #endregion

        #region Static methods

        /// <summary>Cleans a given text to contain only letters or digits.</summary>
        /// <param name="text">Text to clean.</param>
        /// <param name="removePunctuation">Remove punctuation from text (default: true, optional).</param>
        /// <returns>Clean text with only letters and digits.</returns>
        public static string CleanText(string text, bool removePunctuation = true)
        {
            char[] arr = text.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
               || char.IsWhiteSpace(c)
               || (!removePunctuation && char.IsPunctuation(c))
            //|| c == '-'
            )));

            //return cleanStringRegex.Replace(text, " ");
            return ClearSpaces(new string(arr));
        }

        /// <summary>Cleans a given text from multiple spaces.</summary>
        /// <param name="text">Text to clean.</param>
        /// <returns>Clean text without multiple spaces.</returns>
        public static string ClearSpaces(string text)
        {

            return cleanSpacesRegex.Replace(text, " ").Trim();
        }

        /// <summary>Validates a given path and add missing slash.</summary>
        /// <param name="path">Path to validate</param>
        /// <returns>Valid path</returns>
        public static string ValidatePath(string path)
        {
            string result;

            if (isWindowsPlatform)
            {
                result = path.Replace('/', '\\');

                if (!result.EndsWith(WINDOWS_PATH_DELIMITER))
                {
                    result += WINDOWS_PATH_DELIMITER;
                }
            }
            else
            {
                result = path.Replace('\\', '/');

                if (!result.EndsWith(UNIX_PATH_DELIMITER))
                {
                    result += UNIX_PATH_DELIMITER;
                }
            }

            return result;
        }

        /// <summary>Split the given text to lines and return it as list.</summary>
        /// <param name="text">Complete text fragment</param>
        /// <returns>Splitted lines as array</returns>
        public static List<string> SplitStringToLines(string text)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'SplitStringToLines()' will return an empty string list.");
            }
            else
            {
                string[] lines = lineEndingsRegex.Split(text);

                for (int ii = 0; ii < lines.Length; ii++)
                {
                    if (!String.IsNullOrEmpty(lines[ii]))
                    {
                        result.Add(lines[ii]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generate nice HSV colors.
        /// Based on https://gist.github.com/rje/6206099
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Value</param>
        /// <param name="a">Alpha (optional)</param>
        /// <returns>True if the current platform is supported.</returns>
        public static Color HSVToRGB(float h, float s, float v, float a = 1f)
        {
            if (s == 0)
            {
                return new Color(v, v, v, a);
            }

            float hue = h / 60f;
            int sector = Mathf.FloorToInt(hue);
            float fact = hue - sector;
            float p = v * (1f - s);
            float q = v * (1f - s * fact);
            float t = v * (1f - s * (1f - fact));

            switch (sector)
            {
                case 0:
                    return new Color(v, t, p, a);
                case 1:
                    return new Color(q, v, p, a);
                case 2:
                    return new Color(p, v, t, a);
                case 3:
                    return new Color(p, q, v, a);
                case 4:
                    return new Color(t, p, v, a);
                default:
                    return new Color(v, p, q, a);
            }
        }

        /// <summary>Marks the current word or all spoken words from a given text array.</summary>
        /// <param name="speechTextArray">Array with all text fragments</param>
        /// <param name="wordIndex">Current word index</param>
        /// <param name="markAllSpokenWords">Mark the spoken words (default: false, optional)</param>
        /// <param name="markPrefix">Prefix for every marked word (default: green, optional)</param>
        /// <param name="markPostfix">Postfix for every marked word (default: green, optional)</param>
        /// <returns>Marked current word or all spoken words.</returns>
        public static string MarkSpokenText(string[] speechTextArray, int wordIndex, bool markAllSpokenWords = false, string markPrefix = "<color=green>", string markPostfix = "</color>")
        {
            StringBuilder sb = new StringBuilder();

            if (speechTextArray == null)
            {
                Debug.LogWarning("The given 'speechTextArray' is null!");
            }
            else
            {
                if (wordIndex < 0 || wordIndex > speechTextArray.Length - 1)
                {
                    Debug.LogWarning("The given 'wordIndex' is invalid: " + wordIndex);
                }
                else
                {
                    for (int ii = 0; ii < wordIndex; ii++)
                    {

                        if (markAllSpokenWords)
                            sb.Append(markPrefix);
                        sb.Append(speechTextArray[ii]);
                        if (markAllSpokenWords)
                            sb.Append(markPostfix);
                        sb.Append(" ");
                    }

                    sb.Append(markPrefix);
                    sb.Append(speechTextArray[wordIndex]);
                    sb.Append(markPostfix);
                    sb.Append(" ");

                    for (int ii = wordIndex + 1; ii < speechTextArray.Length; ii++)
                    {
                        sb.Append(speechTextArray[ii]);
                        sb.Append(" ");
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Extension method for Arrays.
        /// Dumps an array to a string.
        /// </summary>
        /// <param name="array">Array to dump.</param>
        /// <returns>String with lines for all array entries.</returns>
        public static string CTDump<T>(this T[] array)
        {
            if (array == null || array.Length <= 0)
                throw new ArgumentNullException("array");

            StringBuilder sb = new StringBuilder();

            foreach (T element in array)
            {
                if (0 < sb.Length)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(element.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extension method for Lists.
        /// Dumps a list to a string.
        /// </summary>
        /// <param name="list">List to dump.</param>
        /// <returns>String with lines for all list entries.</returns>
        public static string CTDump<T>(this List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            StringBuilder sb = new StringBuilder();

            foreach (T element in list)
            {
                if (0 < sb.Length)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(element.ToString());
            }

            return sb.ToString();
        }

        #endregion

    }
}
// Copyright 2015-2016 www.crosstales.com