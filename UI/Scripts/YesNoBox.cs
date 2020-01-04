using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    public class YesNoBox : MonoBehaviour
    {

        private Image _yes;
        private Image _no;

        private Image Yes
        {
            get
            {
                if (_yes == null) _yes = transform.Find("Yes").GetComponent<Image>();
                return _yes;
            }
        }

        private Image No
        {
            get
            {
                if (_no == null) _no = transform.Find("No").GetComponent<Image>();
                return _no;
            }
        }

        private bool _isYes;

        public bool IsYes
        {
            get => _isYes;
            set
            {
                _isYes = value;
                Yes.enabled = _isYes;
                No.enabled = !_isYes;
            }
        }

        public bool ValueOnAwake;

        public void Awake()
        {
            _yes = transform.Find("Yes").GetComponent<Image>();
            _no = transform.Find("No").GetComponent<Image>();

            IsYes = ValueOnAwake;
        }
    }
}