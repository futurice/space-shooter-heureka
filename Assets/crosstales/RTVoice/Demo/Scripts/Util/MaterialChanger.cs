using UnityEngine;
using System.Collections;

namespace Crosstales.RTVoice.Demo.Util
{
    /// <summary>Changes the material of a renderer while an AudioSource is playing.</summary>
    [RequireComponent(typeof(Renderer))]
    public class MaterialChanger : MonoBehaviour
    {

        #region Variables
        public AudioSource Source;
        public Material ActiveMaterial;

        private Material inactiveMaterial;
        private Renderer myRenderer;

        #endregion

        #region MonoBehaviour methods

        void Start()
        {
            myRenderer = GetComponent<Renderer>();

            inactiveMaterial = myRenderer.material;
        }

        void Update()
        {
            if (Source.isPlaying)
            {
                myRenderer.material = ActiveMaterial;
            }
            else
            {
                myRenderer.material = inactiveMaterial;
            }
        }

        #endregion
    }
}
// Copyright 2016 www.crosstales.com