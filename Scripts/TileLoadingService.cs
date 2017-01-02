using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class TileLoadingService
    {
        private LoadServiceWaitCallback loadServiceWaitCallback;
        private bool busy;

        public TileLoadingService(LoadServiceWaitCallback loadServiceWaitCallback)
        {
            this.loadServiceWaitCallback = loadServiceWaitCallback;

        }

        public IEnumerator Load(string url)
        {
            while(busy)
            {
                yield return null;
            }
            loadServiceWaitCallback(BusyLock);
            var dataLoader = new MapDataLoader(url);
            IEnumerator result = null;
            yield return result = dataLoader.Load();
            yield return result.Current;
        }

        private IEnumerator BusyLock(float delay)
        {
            busy = true;
            yield return new WaitForSeconds(delay);
            busy = false;
        }
    }
}