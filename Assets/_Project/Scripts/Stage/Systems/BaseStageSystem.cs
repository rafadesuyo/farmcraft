using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    public abstract class BaseStageSystem : MonoBehaviour
    {
        public bool IsReady { get; protected set; }
        protected abstract void RegisterSystem();
        protected abstract void UnregisterSystem();
        public virtual void Initialize() { }

        protected virtual void Awake()
        {
            RegisterSystem();
        }

        protected virtual void OnDestroy()
        {
            UnregisterSystem();
        }
    }
}