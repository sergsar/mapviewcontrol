using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapTileUpdater
    {
        private List<MapTile> tiles = new List<MapTile>();
        private float requestRateLimit = 0.1F;

        private float latitude = 55.75275F;
        private float longitude = 37.62074F;
        private int zoomLevel = 11;
        private int resolution = 350;

        private void RemoveAll(MapTile tile)
        {
            tiles.RemoveAll(p => p == tile);
        }

        public void Queue(MapTile tile)
        {
            if(tiles.Contains(tile))
            {
                RemoveAll(tile);
            }
            tiles.Add(tile);
        }

        public void Discard(MapTile tile)
        {
            RemoveAll(tile);
        }

        public IEnumerator KeepWatch()
        {
            while (true)
            {
                yield return new WaitForSeconds(requestRateLimit);
                if (tiles.Count > 0)
                {
                    var tile = tiles[tiles.Count - 1];
                    RemoveAll(tile);

                    var mapRequest = new YandexMapRequest(latitude, longitude, zoomLevel, resolution);
                    var mapDataLoader = new MapDataLoader(mapRequest.GetUrl());
                    IEnumerator result = null;
                    yield return result = mapDataLoader.Load();
                    var data = result.Current as byte[];
                    if (tile.gameObject && data != null)
                    {
                        var applier = new MapTileApplier();
                        applier.Apply(tile.gameObject, data, resolution);
                    }
                }
            }
        }
    }
}