using UnityEditor;
using Crosstales.RTVoice.Tool;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Custom editor for the 'Sequencer'-class.</summary>
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!Speaker.isTTSAvailable)
            {
                EditorHelper.SeparatorUI();

                EditorHelper.NoVoicesUI();
            }
        }
    }
}
// Copyright 2016 www.crosstales.com