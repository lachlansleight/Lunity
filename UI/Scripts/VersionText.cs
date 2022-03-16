using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    [RequireComponent(typeof(Text))]
    public class VersionText : MonoBehaviour
    {
        public string Prefix = "v";

        // Start is called before the first frame update
        public void Awake()
        {
            GetComponent<Text>().text = Prefix + Application.version;
        }
    }
}