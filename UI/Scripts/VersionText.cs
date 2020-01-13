using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionText : MonoBehaviour
{

    public bool PrependVersion = false;
    
    // Start is called before the first frame update
    public void Awake()
    {
        GetComponent<Text>().text = (PrependVersion ? "Version " : "") + Application.version;
    }
}
