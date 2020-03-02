using System;
using System.Collections;
using System.Collections.Generic;
using Lunity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lunity
{

    public class ColorPickerPopup : MonoBehaviour
    {
        private static ColorPickerPopup _instance;

        public static ColorPickerPopup Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<ColorPickerPopup>();
                return _instance;
            }
        }
        
        public ColorPicker Target;

        private RectTransform _myRt;

        private RectTransform _mainRt;
        private RectTransform _mainMarkerRt;
        private Material _mainMat;

        private RectTransform _h1Rt;
        private RectTransform _h1MarkerRt;
        private Material _h1Mat;

        private RectTransform _h2Rt;
        private RectTransform _h2MarkerRt;
        private Material _h2Mat;

        private RectTransform _vRt;
        private RectTransform _vMarkerRt;
        private Material _vMat;

        private Image _colorPreviewImage;

        private bool _draggingMain;
        private bool _draggingH1;
        private bool _draggingH2;
        private bool _draggingV;

        private float _h;
        private float _s;
        private float _v;
        private static readonly int Hue = Shader.PropertyToID("_Hue");
        private static readonly int Value = Shader.PropertyToID("_Value");
        private static readonly int Saturation = Shader.PropertyToID("_Saturation");

        private Color _sentColor;

        private bool _hasComponents;

        public static void CreateInstance()
        {
            _instance = FindObjectOfType<ColorPickerPopup>();
        }

        public void GetComponents()
        {
            _myRt = GetComponent<RectTransform>();

            _mainRt = transform.Find("RawImage_Main").GetComponent<RectTransform>();
            _mainMarkerRt = transform.Find("RawImage_Main/Marker").GetComponent<RectTransform>();
            _mainMat = _mainRt.GetComponent<RawImage>().material;

            _h1Rt = transform.Find("RawImage_H1").GetComponent<RectTransform>();
            _h1MarkerRt = transform.Find("RawImage_H1/Marker").GetComponent<RectTransform>();
            _h1Mat = _mainRt.GetComponent<RawImage>().material;

            _h2Rt = transform.Find("RawImage_H2").GetComponent<RectTransform>();
            _h2MarkerRt = transform.Find("RawImage_H2/Marker").GetComponent<RectTransform>();
            _h2Mat = _h2Rt.GetComponent<RawImage>().material;

            _vRt = transform.Find("RawImage_V").GetComponent<RectTransform>();
            _vMarkerRt = transform.Find("RawImage_V/Marker").GetComponent<RectTransform>();
            _vMat = _vRt.GetComponent<RawImage>().material;

            _colorPreviewImage = transform.Find("ColorPreviewImage").GetComponent<Image>();

            _hasComponents = true;
            //Initialize(null, Color.red, null, null);
        }

        public void Initialize(ColorPicker target)
        {
            gameObject.SetActive(true);
            
            if (!_hasComponents) GetComponents();

            //place picker popup in the rect transform provided to it
            Target = target;
            _myRt.SetParent(Target.PopupRectTransform, false);
            _myRt.anchorMin = Vector2.zero;
            _myRt.anchorMax = Vector2.one;
            _myRt.offsetMin = Vector2.zero;
            _myRt.offsetMax = Vector2.zero;

            _colorPreviewImage.color = target.CurrentColor;
            Color.RGBToHSV(target.CurrentColor, out var h, out var s, out var v);
            _h = h;
            _s = s;
            _v = v;
            UpdateMaterials();
            UpdateMarkers();
        }

        public void HandleMainDrag(BaseEventData data)
        {
            var point = GetRelativePosition(_mainRt, data.currentInputModule.input.mousePosition);
            _s = point.x;
            _v = point.y;
            UpdateMaterials();
            UpdateMarkers();
        }

        public void HandleH1Drag(BaseEventData data)
        {
            var point = GetRelativePosition(_h1Rt, data.currentInputModule.input.mousePosition);
            _s = point.x;
            UpdateMaterials();
            UpdateMarkers();
        }

        public void HandleH2Drag(BaseEventData data)
        {
            var point = GetRelativePosition(_h2Rt, data.currentInputModule.input.mousePosition);
            _v = point.x;
            UpdateMaterials();
            UpdateMarkers();
        }

        public void HandleVDrag(BaseEventData data)
        {
            var point = GetRelativePosition(_vRt, data.currentInputModule.input.mousePosition);
            _h = point.y;
            UpdateMaterials();
            UpdateMarkers();
        }

        private Vector2 GetRelativePosition(RectTransform rt, Vector2 mousePos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rt,
                mousePos,
                null,
                out var point);

            var size = rt.rect.size;
            return new Vector2(Mathf.Clamp01((point.x / size.x) + rt.pivot.x), Mathf.Clamp01((point.y / size.y) + rt.pivot.y));
        }

        private void UpdateMaterials()
        {
            _mainMat.SetFloat(Hue, _h);

            _h1Mat.SetFloat(Hue, _h);
            _h1Mat.SetFloat(Value, _v);

            _h2Mat.SetFloat(Hue, _h);
            _h2Mat.SetFloat(Saturation, _s);

            _colorPreviewImage.color = Color.HSVToRGB(_h, _s, _v);

            if (_colorPreviewImage.color != _sentColor) {
                _sentColor = _colorPreviewImage.color;
                Target.HandleColorChange(_sentColor);
            }
        }

        private void UpdateMarkers()
        {
            var size = _mainRt.rect.size;
            _mainMarkerRt.anchoredPosition = new Vector2(_s * size.x, _v * size.y);

            size = _h1Rt.rect.size;
            _h1MarkerRt.anchoredPosition = new Vector2(_s * size.x, 0);

            size = _h2Rt.rect.size;
            _h2MarkerRt.anchoredPosition = new Vector2(_v * size.x, 0);

            size = _vRt.rect.size;
            _vMarkerRt.anchoredPosition = new Vector2(0, _h * size.y);
        }

        
        private void Update()
        {
            //if the user clicks outside of the color picker window, close it!
            if (Input.GetMouseButtonDown(0)) {
                var relativePos = GetRelativePosition(_myRt, Input.mousePosition);
                //Debug.Log(relativePos);
                if (relativePos.x <= 0f || relativePos.x >= 1f || relativePos.y <= 0f || relativePos.y >= 1f)
                    CloseWindow();
            }
        }
        

        public void CloseWindow()
        {
            Target.HandleWindowClose();
            Target = null;
            gameObject.SetActive(false);
        }
    }
}