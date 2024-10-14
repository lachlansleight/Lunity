using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class LTime : MonoBehaviour
    {
        /// Returns the current unix timestamp (in milliseconds)
        public static long Now => DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}