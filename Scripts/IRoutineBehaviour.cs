using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public interface IRoutineBehaviour
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}