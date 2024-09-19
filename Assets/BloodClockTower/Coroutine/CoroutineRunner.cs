using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodClockTower
{
    public class CoroutineRunner : MonoBehaviour
    {
        public void Run(IEnumerator coroutine, Action? callback = null)
        {
            StartCoroutine(RunInternal(coroutine, callback));
        }
    
        private IEnumerator RunInternal(IEnumerator coroutine, Action? callback = null)
        {
            yield return StartCoroutine(coroutine);
            callback?.Invoke();
        }
    }
}