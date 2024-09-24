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
            var safeArea = Screen.safeArea;
            var topInset = (Screen.height - safeArea.yMax);
            var bottomInset = safeArea.y;
            var leftInset = safeArea.x;
            var rightInset = (Screen.width - safeArea.xMax);
            var rt = (RectTransform) transform;
            rt.offsetMin = new Vector2(leftInset, bottomInset);
            rt.offsetMax = new Vector2(-rightInset, -topInset);
        }
    }
}