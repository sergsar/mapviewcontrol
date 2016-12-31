using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class TileLoadingService
    {
        private ICoroutineExecutor coroutineExecutor;
        private Dictionary<MapTile, string> pendingTiles;
        private Dictionary<MapTile, byte[]> receivedTiles;

        public TileLoadingService(ICoroutineExecutor coroutineExecutor)
        {
            this.coroutineExecutor = coroutineExecutor;

            coroutineExecutor.StartCoroutine(DispatchCycle());
        }

        public void Request(MapTile mapTile, string url)
        {
            pendingTiles[mapTile] = url;
        }

        private IEnumerator DispatchCycle()
        {
            while (true)
            {
                if (pendingTiles.Count == 0)
                {
                    continue;
                }
                var keyValuePair = pendingTiles.LastOrDefault();
                var tile = keyValuePair.Key;
                var url = keyValuePair.Value;
                var mapDataLoader = new MapDataLoader(url);
                IEnumerator result = null;
                yield return result = mapDataLoader.Load();
                if (tile == null || tile.gameObject == null)
                {
                    continue;
                }
                var data = result.Current as byte[];
                pendingTiles.Remove(tile);
                receivedTiles[tile] = data;
            }
        }
    }
}