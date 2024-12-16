using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EBTutorial
{
    public class MonoEventHelper : MonoBehaviour
    {

        public event Action OnAwakeEvent;
        public event Action OnStartEvent;
        public event Action<bool> OnEnableEvent;
        public event Action OnDestroyEvent;

        private void Awake()
        {
            OnAwakeEvent?.Invoke();
        }

        private void Start()
        {
            OnStartEvent?.Invoke();
        }

        private void OnEnable()
        {
            OnEnableEvent?.Invoke(true);
        }

        private void OnDisable()
        {
            OnEnableEvent?.Invoke(false);
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }

}
