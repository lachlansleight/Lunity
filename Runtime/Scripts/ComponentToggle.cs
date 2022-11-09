using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentToggle : MonoBehaviour
{
    public Behaviour TargetBehaviour;
    public KeyCode ToggleKey = KeyCode.ScrollLock;

    public void Update()
    {
        if (Input.GetKeyDown(ToggleKey)) TargetBehaviour.enabled = !TargetBehaviour.enabled;
    }
}
