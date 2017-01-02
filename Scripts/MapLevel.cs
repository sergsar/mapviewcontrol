using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevel : MonoBehaviour, ITranslatable
    {
        private List<MapTile> tiles = new List<MapTile>();
        private TileUpdaterCallback tileUpdaterCallback;
        private UnityEngine.Object tileRefObject;
        private MapContext mapContext;

        public TileUpdaterCallback TileUpdaterCallback { set { tileUpdaterCallback = value; } }
        public UnityEngine.Object TileRefObject { set { tileRefObject = value; } }

        public MapContext MapContext { set { mapContext = value; } }

        public void Construct(PixelLocation initLocation, int zoomLevel)
        {
            var converter = new MapPixelConverter();
            var tileStep = mapContext.TileResolution * converter.GetZoomMultiplier(zoomLevel);
            var cut = mapContext.Cut;
            var levelStep = tileStep * cut;
            var tileScale = 1F / cut;
            Func<int, float> place = (index) => (tileScale - 1F) * 0.5F + tileScale * index;
            Func<int, int, int> locationPlace = (center, index) => (int)(center - (levelStep + tileStep) * 0.5F + tileStep * index);
            var mapLevelContext = new MapLevelContext(zoomLevel, levelStep, tileScale);
            var instanciateCallback = new InstanciateCallback(() => Instantiate(tileRefObject));
            var mapTileFactory = new MapTileFactory(instanciateCallback, mapLevelContext, tileUpdaterCallback);

            for (var x = 0; x < cut; ++x)
            {
                for (var z = 0; z < cut; ++z)
                {
                    var location = new PixelLocation() { X = locationPlace(initLocation.X, x), Z = locationPlace(initLocation.Z, z) };
                    var tile = mapTileFactory.GetMapTile();
                    tile.gameObject.SetParent(gameObject);
                    tile.transform.localPosition = new Vector3(place(x), 0F, place(z));
                    tile.transform.localScale = Vector3.one * tileScale;
                    tile.Construct(location);
                    tiles.Add(tile);
                }
            }
        }

        public void Translate(Vector3 difference)
        {
            tiles.ForEach(p => p.Translate(difference));
        }
    }
}