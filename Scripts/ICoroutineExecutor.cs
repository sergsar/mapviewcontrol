
namespace MapViewScripts
{
    public interface ICoroutineExecutor
    {
        UnityEngine.Coroutine StartCoroutine(System.Collections.IEnumerator routine);
    }
}