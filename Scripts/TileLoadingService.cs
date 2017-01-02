using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class TileLoadingService
    {
        private ICoroutineExecutor coroutineExecutor;
        private Dictionary<MapTile, string> pendingTiles = new Dictionary<MapTile, string>();
        private Dictionary<MapTile, byte[]> receivedTiles = new Dictionary<MapTile, byte[]>();
        private Dictionary<MapTile, MapDataLoader> processTiles = new Dictionary<MapTile, MapDataLoader>();
        private float delay = 1F;

        public TileLoadingService(ICoroutineExecutor coroutineExecutor)
        {
            this.coroutineExecutor = coroutineExecutor;

            coroutineExecutor.StartCoroutine(SendingCycle());
            coroutineExecutor.StartCoroutine(ReceivingCycle());
        }

        public byte[] Load(MapTile mapTile, string url)
        {
            if(processTiles.ContainsKey(mapTile))
            {
                processTiles.Remove(mapTile);
            }
            pendingTiles[mapTile] = url;
            if (receivedTiles.ContainsKey(mapTile))
            {
                return receivedTiles[mapTile];
            }
            return null;
        }

        private IEnumerator SendingCycle()
        {
            while (true)
            {
                yield return null;
                if (pendingTiles.Count == 0)
                {
                    continue;
                }
                var pending = pendingTiles.LastOrDefault();
                var tile = pending.Key;
                pendingTiles.Remove(tile);
                if (tile == null || tile.gameObject == null)
                {
                    continue;
                }
                var url = pending.Value;
                var mapDataLoader = new MapDataLoader(url);
                processTiles[tile] = mapDataLoader;
                mapDataLoader.Send();
                yield return new WaitForSeconds(delay);
            }
        }

        private IEnumerator ReceivingCycle()
        {
            while (true)
            {
                var processed = processTiles.Where(p => p.Value.Processed).FirstOrDefault();
                var tile = processed.Key;
                processTiles.Remove(tile);
                yield return null;
                if (tile == null || tile.gameObject == null)
                {
                    continue;
                }
                var result = processed.Value.TakeData();
                receivedTiles[tile] = result;
            }
        }
    }
}