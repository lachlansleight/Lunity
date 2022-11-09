using System.Collections;
using System.Collections.Generic;
using Lunity;
using UnityEngine;

public class RootCanvas : SimpleSingleton<RootCanvas>
{
    public static Canvas Find
    {
        get
        {
            var instance = Instance;
            if (instance) return instance.GetComponent<Canvas>();
            throw new UnityException("No RootCanvas component found. Put it on your root canvas!");
        }
    }
}
