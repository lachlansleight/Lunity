using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lunity
{
    public class RectTransformContextMenu
    {
        [MenuItem("CONTEXT/RectTransform/Fill/Left Half")]
        private static void FillLeft(MenuCommand command)
        {
            var rt = (RectTransform) command.context;
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0.5f, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        
        [MenuItem("CONTEXT/RectTransform/Fill/Right Half")]
        private static void FillRight(MenuCommand command)
        {
            var rt = (RectTransform) command.context;
            rt.anchorMin = new Vector2(0.5f, 0);
            rt.anchorMax = new Vector2(1f, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        
        [MenuItem("CONTEXT/RectTransform/Fill/Top Half")]
        private static void FillTop(MenuCommand command)
        {
            var rt = (RectTransform) command.context;
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        
        [MenuItem("CONTEXT/RectTransform/Fill/Bottom Half")]
        private static void FillBottom(MenuCommand command)
        {
            var rt = (RectTransform) command.context;
            rt.anchorMin = new Vector2(0, 0f);
            rt.anchorMax = new Vector2(1f, 0.5f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void Place(RectTransform rt, float x, float y)
        {
            var width = rt.rect.width;
            var height = rt.rect.height;
            var v = new Vector2(x, y);
            rt.pivot = v;
            rt.anchorMin = v;
            rt.anchorMax = v;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        
        //These are similar to the default Unity ones, but they also change the object's pivot which makes it more intuitive to move
        [MenuItem("CONTEXT/RectTransform/Position/TopLeft")]
        private static void PlaceTopLeft(MenuCommand command) { Place((RectTransform) command.context, 0f, 1f); }
        [MenuItem("CONTEXT/RectTransform/Position/TopCenter")]
        private static void PlaceTopCenter(MenuCommand command) { Place((RectTransform) command.context, 0.5f, 1f); }
        [MenuItem("CONTEXT/RectTransform/Position/TopRight")]
        private static void PlaceTopRight(MenuCommand command) { Place((RectTransform) command.context, 1f, 1f); }
        
        [MenuItem("CONTEXT/RectTransform/Position/MiddleLeft")]
        private static void PlaceMiddleLeft(MenuCommand command) { Place((RectTransform) command.context, 0f, 0.5f); }
        [MenuItem("CONTEXT/RectTransform/Position/MiddleCenter")]
        private static void PlaceMiddleCenter(MenuCommand command) { Place((RectTransform) command.context, 0.5f, 0.5f); }
        [MenuItem("CONTEXT/RectTransform/Position/MiddleRight")]
        private static void PlaceMiddleRight(MenuCommand command) { Place((RectTransform) command.context, 1f, 0.5f); }
        
        [MenuItem("CONTEXT/RectTransform/Position/BottomLeft")]
        private static void PlaceBottomLeft(MenuCommand command) { Place((RectTransform) command.context, 0f, 0f); }
        [MenuItem("CONTEXT/RectTransform/Position/BottomCenter")]
        private static void PlaceBottomCenter(MenuCommand command) { Place((RectTransform) command.context, 0.5f, 0f); }
        [MenuItem("CONTEXT/RectTransform/Position/BottomRight")]
        private static void PlaceBottomRight(MenuCommand command) { Place((RectTransform) command.context, 1f, 0f); }
    }
}