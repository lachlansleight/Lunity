using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public interface IInitializable
    {
        void Initialize();
        bool Initialized { get; set; }
    }
}