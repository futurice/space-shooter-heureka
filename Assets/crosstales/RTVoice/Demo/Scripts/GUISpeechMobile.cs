using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Util;

namespace Crosstales.RTVoice.Demo
{
    /// <summary>Simple GUI for runtime TTS with all available OS voices.</summary>
    public class GUISpeechMobile : MonoBehaviour
    {

        #region Variables

        public GameObject ItemPrefab;
        public GameObject Target;
        public Scrollbar Scroll;
        public int ColumnCount = 1;
        public Vector2 SpaceWidth = new Vector2(8, 8);
        public Vector2 SpaceHeight = new Vector2(8, 8);
        public InputField Input;
        public InputField Culture;
        public Text Cultures;
        public static float Rate = 1f;
        public static float Pitch = 1f;
        public static float Volume = 1f;
        public static bool isNative = false;

        private string lastCulture = "unknown";
        private List<SpeakWrapper> wrappers = new List<SpeakWrapper>();

        private bool buildVoicesListIOS = false;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            Cultures.text = string.Join(", ", Speaker.Cultures.ToArray());
            Input.text = "Hi there, my name is RTVoice, your runtime speaker!";
            Culture.text = string.Empty;

            //buildVoicesList();
        }

        void Update()
        {
            if (!lastCulture.Equals(Culture.text))
            {
                buildVoicesList();

                lastCulture = Culture.text;
            }

            if (Helper.isIOSPlatform && Time.frameCount % 30 == 0 && !buildVoicesListIOS)
            {
                buildVoicesList();
                buildVoicesListIOS = true;
            }
        }

        #endregion

        #region Public methods

        public void Silence()
        {
            //foreach (SpeakWrapper wrapper in wrappers)
            //{
            //    if (wrapper.Audio != null)
            //    {
            //        wrapper.Audio.Stop();
            //        wrapper.Audio.clip = null;
            //    }
            //}

            Speaker.Silence();
        }

        public void ChangeRate(float rate)
        {
            Rate = rate;
        }

        public void ChangeVolume(float volume)
        {
            Volume = volume;
        }

        public void ChangePitch(float pitch)
        {
            Pitch = pitch;
        }

        public void ChangeNative(bool native)
        {
            isNative = native;
        }
        #endregion

        #region Private methods

        private void buildVoicesList()
        {
            wrappers.Clear();

            RectTransform rowRectTransform = ItemPrefab.GetComponent<RectTransform>();
            RectTransform containerRectTransform = Target.GetComponent<RectTransform>();

            for (var i = Target.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = Target.transform.GetChild(i);
                child.SetParent(null);
                Destroy(child.gameObject);
                // Optionally destroy the objectA if not longer needed
            }


            List<Voice> items = Speaker.VoicesForCulture(Culture.text);

            if (items.Count == 0)
            {
                Debug.LogWarning("No voices for culture '" + Culture.text + "' found - using the default system voices.");
                items = Speaker.Voices;
            }

            if (items.Count > 0)
            {
                //Debug.Log("ITEMS: " + items.Count);

                //calculate the width and height of each child item.
                float width = containerRectTransform.rect.width / ColumnCount - (SpaceWidth.x + SpaceWidth.y) * ColumnCount;
                float height = rowRectTransform.rect.height - (SpaceHeight.x + SpaceHeight.y);

                int rowCount = items.Count / ColumnCount;

                if (rowCount > 0 && items.Count % rowCount > 0)
                {
                    rowCount++;
                }

                //adjust the height of the container so that it will just barely fit all its children
                float scrollHeight = height * rowCount;
                containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
                containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

                int j = 0;
                for (int ii = 0; ii < items.Count; ii++)
                {
                    //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
                    if (ii % ColumnCount == 0)
                    {
                        j++;
                    }

                    //create a new item, name it, and set the parent
                    GameObject newItem = Instantiate(ItemPrefab) as GameObject;
                    newItem.name = Target.name + " item at (" + ii + "," + j + ")";
                    newItem.transform.SetParent(Target.transform);
                    newItem.transform.localScale = Vector3.one;

                    SpeakWrapper wrapper = newItem.GetComponent<SpeakWrapper>();
                    wrapper.SpeakerVoice = items[ii];
                    wrapper.Input = Input;
                    wrapper.Label.text = items[ii].Name;
                    wrappers.Add(wrapper);

                    //move and size the new item
                    RectTransform rectTransform = newItem.GetComponent<RectTransform>();

                    float x = -containerRectTransform.rect.width / 2 + (width + SpaceWidth.x) * (ii % ColumnCount) + SpaceWidth.x * ColumnCount;
                    float y = containerRectTransform.rect.height / 2 - height * j;
                    rectTransform.offsetMin = new Vector2(x, y);

                    x = rectTransform.offsetMin.x + width;
                    y = rectTransform.offsetMin.y + height;
                    rectTransform.offsetMax = new Vector2(x, y);
                }

            }
            else
            {
                Debug.LogError("No voices found - speech is not possible!");
            }

            Scroll.value = 1f;
        }

        #endregion
    }
}
// Copyright 2015-2016 www.crosstales.com