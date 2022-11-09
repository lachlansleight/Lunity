using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Lunity
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class ColorPicker : MonoBehaviour
    {
        public RectTransform PopupRectTransform;
        public UnityColorEvent OnColorChange;

        public Color CurrentColor
        {
            get
            {
                if (_colorPreviewImage == null) _colorPreviewImage = GetComponent<Image>();
                return _colorPreviewImage.color;
            }
            set
            {
                if (_colorPreviewImage == null) _colorPreviewImage = GetComponent<Image>();
                HandleColorChange(value);
            }
        }
        
        private Image _colorPreviewImage;
        private ColorPickerPopup _popup;

        private bool _popupVisible;

        public void Awake()
        {
            _colorPreviewImage = GetComponent<Image>();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        
        public void OnClick()
        {
            if (_popup == null) {
                _popup = ColorPickerPopup.Instance;
                //if still null, that means that this is the first time a Color Picker has been clicked this scene
                //which means we need to create one and assign the static instance!
                if (_popup == null) {
                    var newObj = Instantiate(Resources.Load("ColorPickerPopup")) as GameObject;
                    _popup = newObj.GetComponent<ColorPickerPopup>();
                    ColorPickerPopup.CreateInstance();
                }
            }
            
            if (_popup.Target == this) {
                _popup.CloseWindow();
            } else if (_popup.Target == null) {
                _popup.Initialize(this);
            } else {
                _popup.CloseWindow();
                _popup.Initialize(this);
            }
        }

        public void HandleWindowClose()
        {
            
        }

        public void HandleColorChange(Color color)
        {
            _colorPreviewImage.color = color;
            OnColorChange?.Invoke(color);
        }
    }
}