using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevel : MonoBehaviour
    {
        private List<MapTile> tiles = new List<MapTile>();

        

        private MapTileUpdater mapTileUpdater = new MapTileUpdater();

        public void Construct(UnityEngine.Object tileObject, int cut)
        {
            StartCoroutine(mapTileUpdater.KeepWatch());

            var scale = 1F / cut;
            Func<int, float> place = (p) => (scale - 1F) * 0.5F + scale * p;

            for (var x = 0; x < cut; ++x)
            {
                for (var z = 0; z < cut; ++z)
                {
                    var tile = (GameObject)Instantiate(tileObject);
                    tile.SetParent(gameObject);
                    tile.transform.localPosition = new Vector3(place(x), 0F, place(z));
                    tile.transform.localScale = Vector3.one * scale;
                    var tileComponent = tile.AddComponent<MapTile>();
                    tileComponent.Construct(scale);
                    tiles.Add(tileComponent);
                    mapTileUpdater.Queue(tileComponent);
                }
            }
        }

        public void Translate(Vector3 difference)
        {
            tiles.ForEach(p => p.Translate(difference));
        }
    }
}