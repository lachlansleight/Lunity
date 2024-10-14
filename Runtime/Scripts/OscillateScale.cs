using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class OscillateScale : MonoBehaviour
    {
        public Vector3 Amplitude = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 Period = Vector3.one;
        public Vector3 Center = Vector3.one;
        public Vector3 Phase = Vector3.zero;

        [Range(0f, 1f)] public float Strength = 1f;
        [Range(0.01f, 1f)] public float Smoothness = 0.3f;

        private Vector3 _t;

        public void Update()
        {
            _t += new Vector3(
                Time.deltaTime / Period.x,
                Time.deltaTime / Period.y,
                Time.deltaTime / Period.z
            );
            _t.x %= 1f;
            _t.y %= 1f;
            _t.z %= 1f;

            var offset = new Vector3(
                Amplitude.x * Mathf.Sin((Mathf.Clamp01(_t.x / Smoothness) + Phase.x) * Mathf.PI * 2f),
                Amplitude.y * Mathf.Sin((Mathf.Clamp01(_t.y / Smoothness) + Phase.y) * Mathf.PI * 2f),
                Amplitude.z * Mathf.Sin((Mathf.Clamp01(_t.z / Smoothness) + Phase.z) * Mathf.PI * 2f)
            );
            transform.localScale = Center + offset * Strength;
        }
    }
}