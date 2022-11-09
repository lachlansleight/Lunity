using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public interface IActivatable
    {
        void Activate();
        void Deactivate();
        bool Activated { get; set; }
        Action OnActivated { get; set; }
        Action OnDeactivated { get; set; }
    }
}