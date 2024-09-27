using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    [DefaultExecutionOrder(-999)]
    public class ActiveOnAwake : MonoBehaviour
    {
        public enum ActiveOnAwakeMode
        {
            SetActive,
            SetInactive,
            Toggle,
            DoNothing
        }

        public ActiveOnAwakeMode Mode = ActiveOnAwakeMode.Toggle;
        public GameObject[] Targets;
        
        public void Awake()
        {
            switch (Mode) {
                case ActiveOnAwakeMode.SetActive:
                    foreach (var target in Targets) target.SetActive(true);
                    break;
                case ActiveOnAwakeMode.SetInactive:
                    foreach (var target in Targets) target.SetActive(false);
                    break;
                case ActiveOnAwakeMode.Toggle:
                    foreach (var target in Targets) target.SetActive(!target.activeSelf);
                    break;
                case ActiveOnAwakeMode.DoNothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}