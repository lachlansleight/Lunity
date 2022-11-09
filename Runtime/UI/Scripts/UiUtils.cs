using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public static class UiUtils
    {
        public static void SetUIParent(this GameObject obj, Transform parent, bool cover = false)
        {
            var rt = (RectTransform) obj.transform;
            if (rt == null) {
                Debug.LogError(obj.name + " doesn't have a RectTransform");
                return;
            }

            var prt = (RectTransform) parent;
            if (prt == null) {
                Debug.LogError("Parent doesn't have a RectTransform");
                return;
            }

            rt.SetParent(prt);
            rt.localPosition = Vector3.zero;
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;

            if (!cover) return;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}