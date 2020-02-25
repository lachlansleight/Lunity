using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public interface IInitializable
    {
        void Initialize();
        void Deinitialize();
        bool Initialized { get; set; }
        Action OnInitialized { get; set; }
        Action OnDeinitialized { get; set; }
    }
}