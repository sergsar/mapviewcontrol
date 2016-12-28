using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace MapViewScripts
{
    public class MapDataLoader
    {
        private string url;

        private byte[] TakeData(UnityWebRequest request)
        {
            if (request.isError)
            {
                Debug.LogError(request.error);
                return null;
            }

            byte[] result = request.downloadHandler.data;
            return result;
        }

        public MapDataLoader(string url)
        {
            this.url = url;
        }

        public IEnumerator Load()
        {
            var request = new UnityWebRequest();
            request.url = url;
            request.redirectLimit = 4;
            request.method = "GET";
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.Send();
            yield return TakeData(request);
        }

    }
}