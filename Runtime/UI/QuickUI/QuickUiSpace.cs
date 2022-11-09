using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickUiSpace : QuickUiSceneControl
{
    public float Size = 10f;

    public void Initialize(float size)
    {
        Size = size;

        var rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0.5f);
        rt.anchorMax = new Vector2(1f, 0.5f);
        rt.offsetMin = new Vector2(0f, -Size);
        rt.offsetMax = new Vector2(0f, Size);

        gameObject.name = "Space";
    }
}
