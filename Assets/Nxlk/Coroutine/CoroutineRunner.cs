using System;
using System.Collections;
using UnityEngine;

namespace Nxlk.Coroutine
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
