using System;
using System.Collections;
using System.Collections.Generic;
using Lunity;
using UnityEngine;

namespace Lunity
{
    public class ActivatableInitializable : MonoBehaviour, IInitializable, IActivatable
    {
        public bool Initialized { get; set; }
        public bool Activated { get; set; }
        public Action OnInitialized { get; set; }
        public Action OnDeinitialized { get; set; }
        public Action OnActivated { get; set; }
        public Action OnDeactivated { get; set; }

        public virtual void Initialize()
        {
            Initialized = true;
            OnInitialized?.Invoke();
        }

        public virtual void Deinitialize()
        {
            Initialized = false;
            OnDeactivated?.Invoke();
        }
        
        public virtual void Activate()
        {
            Activated = true;
            OnActivated?.Invoke();
        }

        public virtual void Deactivate()
        {
            Activated = false;
            OnDeactivated?.Invoke();
        }

    }
}