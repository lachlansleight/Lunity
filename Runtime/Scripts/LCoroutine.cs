using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class LCoroutine
    {
        public static void RunDelayed(MonoBehaviour caller, Action function, int frames = 1)
        {
            caller.StartCoroutine(RunDelayedInternal(function, frames));
        }

        private static IEnumerator RunDelayedInternal(Action function, int frames)
        {
            for (var i = 0; i < frames; i++) yield return null;
            function?.Invoke();
        }
    }
}