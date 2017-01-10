using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class TileLoadingService
    {
        private float waitTime;
        private IRoutineBehaviour routineBehaviour;
        private bool locked;

        public TileLoadingService(IRoutineBehaviour routineBehaviour, float waitTime)
        {
            this.routineBehaviour = routineBehaviour;
            this.waitTime = waitTime;

        }

        public IEnumerator Load(string url)
        {
            //Debug.Log(url);
            while(locked)
            {
                yield return null;
            }
            routineBehaviour.StartCoroutine(WaitingLock(waitTime));
            var dataLoader = new MapDataLoader(url);
            IEnumerator result = null;
            yield return result = dataLoader.Load();
            yield return result.Current;
        }

        private IEnumerator WaitingLock(float delay)
        {
            locked = true;
            yield return new WaitForSeconds(delay);
            locked = false;
        }
    }
}