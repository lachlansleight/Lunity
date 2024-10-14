using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class OscillatePosition : MonoBehaviour
    {

        public Vector3 Amplitude;
        public Vector3 Period = Vector3.one;

        private Vector3 _t;
        private Vector3 _centerPosition;

        public void OnEnable()
        {
            _centerPosition = transform.localPosition;
        }

        public void Update()
        {
            _t += new Vector3(
                Mathf.PI * 2f * Time.deltaTime / Period.x,
                Mathf.PI * 2f * Time.deltaTime / Period.y,
                Mathf.PI * 2f * Time.deltaTime / Period.z
            );
            var offset = new Vector3(
                Amplitude.x * Mathf.Sin(_t.x),
                Amplitude.y * Mathf.Sin(_t.y),
                Amplitude.z * Mathf.Sin(_t.z)
            );
            transform.localPosition = _centerPosition + offset;
        }
    }
}