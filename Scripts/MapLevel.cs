using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevel : MonoBehaviour
    {
        private List<MapTile> tiles = new List<MapTile>();
        private MapTileLoader mapTileLoader;
        private UnityEngine.Object tileRefObject;
        private MapContext mapContext;

        public MapTileLoader MapTileLoader { set { mapTileLoader = value; } }
        public UnityEngine.Object TileRefObject { set { tileRefObject = value; } }

        public MapContext MapContext { set { mapContext = value; } }


        //private MapTileUpdater mapTileUpdater = new MapTileUpdater();

        public void Construct(PixelLocation initLocation, int zoomLevel)
        {
            //StartCoroutine(mapTileUpdater.KeepWatch());
            var converter = new MapPixelConverter();
            var tileStep = mapContext.TileResolution * converter.GetZoomMultiplier(zoomLevel);
            var cut = mapContext.Cut;
            var levelStep = tileStep * cut;
            var scale = 1F / cut;
            Func<int, float> place = (index) => (scale - 1F) * 0.5F + scale * index;
            Func<int, int, int> locationPlace = (center, index) => (int)(center - (levelStep + tileStep) * 0.5F + tileStep * index);

            for (var x = 0; x < cut; ++x)
            {
                for (var z = 0; z < cut; ++z)
                {
                    var location = new PixelLocation() { X = locationPlace(initLocation.X, x), Z = locationPlace(initLocation.Z, z) };
                    var tile = (GameObject)Instantiate(tileRefObject);
                    tile.SetParent(gameObject);
                    tile.transform.localPosition = new Vector3(place(x), 0F, place(z));
                    tile.transform.localScale = Vector3.one * scale;
                    var tileComponent = tile.AddComponent<MapTile>();
                    tileComponent.Construct(scale, location);
                    tiles.Add(tileComponent);
                    //mapTileUpdater.Queue(tileComponent);
                }
            }
        }

        public void Translate(Vector3 difference)
        {
            tiles.ForEach(p => p.Translate(difference));
        }
    }
}