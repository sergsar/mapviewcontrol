using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YandexMapService : MonoBehaviour
{
    private float requestRateLimit = 0.5F;
    private List<byte[]> loaded = new List<byte[]>();
    private List<string> urls = new List<string>();

    private void Start()
    {
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while(true)
        {
            yield return new WaitForSeconds(requestRateLimit);
            if(urls.Count > 0)
            {
                var url = urls[urls.Count - 1];
                urls.RemoveAll(p => p == url);
                StartCoroutine(Send(url));
            }
        }
    }

    private IEnumerator Send(string url)
    {
        yield return null;
    }
}
