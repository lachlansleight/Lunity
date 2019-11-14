using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    [RequireComponent(typeof(Text))]
    public class CopyTargetText : MonoBehaviour
    {
        public Text TargetText;
        
        private Text _text;

        private Text Text
        {
            get
            {
                if (_text == null) _text = GetComponent<Text>();
                return _text;
            }
        }
        
        public void Update()
        {
            Text.text = TargetText.text;
        }
    }
}