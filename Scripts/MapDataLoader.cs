﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace MapViewScripts
{
    public class MapDataLoader
    {
        private UnityWebRequest request;

        public bool Processed { get { return request.isDone; } }

        private byte[] TakeData()
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
            request = new UnityWebRequest();
            request.url = url;
            request.redirectLimit = 4;
            request.method = "GET";
            request.downloadHandler = new DownloadHandlerBuffer();
        }

        public IEnumerator Load()
        {
            yield return request.Send();
            yield return TakeData();
        }
    }
}