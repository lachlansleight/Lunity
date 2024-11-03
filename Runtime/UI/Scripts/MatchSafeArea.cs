using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    /// Fills the safe area of the screen - note that script assumes that it's the child of a rect that fills the display
    /// (e.g. an overlay canvas)
    public class MatchSafeArea : MonoBehaviour
    {
        public void Start()
        {
            var parentRect = ((RectTransform) transform.parent).rect;
            var safeArea = Screen.safeArea;
            var heightFactor = parentRect.height / Screen.height;
            var widthFactor = parentRect.width / Screen.width;
            var topInset = (Screen.height - safeArea.yMax) * heightFactor;
            var bottomInset = safeArea.y * heightFactor;
            var leftInset = safeArea.x * widthFactor;
            var rightInset = (Screen.width - safeArea.xMax) * widthFactor;
            var rt = (RectTransform) transform;
            rt.offsetMin = new Vector2(leftInset, bottomInset);
            rt.offsetMax = new Vector2(-rightInset, -topInset);
        }
    }
}