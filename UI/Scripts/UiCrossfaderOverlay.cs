using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways, RequireComponent(typeof(Image))]
public class UiCrossfaderOverlay : MonoBehaviour
{
    public Color Color = Color.black;
    private Image _image;

    public void Update()
    {
        if (_image == null) _image = GetComponent<Image>();
        if (_image == null) {
            _image = gameObject.AddComponent<Image>();
        }
        
        _image.raycastTarget = false;
        _image.color = Color;
    }
}
