using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevel : MonoBehaviour
    {
        private List<MapTile> tiles = new List<MapTile>();
        private MapTileUpdater mapTileUpdater;
        private UnityEngine.Object tileRefObject;
        private MapViewContext mapViewContext;
        private PixelLocation initLocation;
        private int zoomLevel;
        private MapPixelConverter converter = new MapPixelConverter();
        private float scaleFactor = 1F;

        public MapTileUpdater MapTileUpdater { set { mapTileUpdater = value; } }
        public UnityEngine.Object TileRefObject { set { tileRefObject = value; } }

        public MapViewContext MapViewContext { set { mapViewContext = value; } }
        public int ZoomLevel { get { return zoomLevel; } }

        public void Init(PixelLocation initLocation, int zoomLevel)
        {
            this.initLocation = initLocation;
            this.zoomLevel = zoomLevel;
        }


        public void Construct()
        {
            var tileStep = mapViewContext.TileResolution * converter.GetZoomMultiplier(zoomLevel);
            var cut = mapViewContext.Cut;
            var startLocation = initLocation + new PixelLocation(-1, 1) * (tileStep / 2 * (cut - 1));
            var levelStep = tileStep * cut;
            var tileScale = 1F / cut;
            Func<int, float> place = (index) => (tileScale - 1F) * 0.5F + tileScale * index;
            Func<int, int, int> locationPlace = (center, index) => center + tileStep * index;
            var mapLevelContext = new MapLevelContext(zoomLevel, levelStep, tileScale);
            var mapTileFactory = new MapTileFactory(tileRefObject, mapLevelContext, mapTileUpdater);

            for (var x = 0; x < cut; ++x)
            {
                for (var z = 0; z < cut; ++z)
                {
                    var location = new PixelLocation() { X = locationPlace(startLocation.X, x), Z = locationPlace(startLocation.Z, -z) };
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

        public void Scale(float delta)
        {
            transform.localScale = Vector3.one * scaleFactor * (delta + 1);
        }

        public void UpdateScaleFactor(float pow)
        {
            scaleFactor *= Mathf.Pow(2F, pow);
        }

        public PixelLocation GetCenterLocation()
        {
            var tile = tiles.FirstOrDefault();
            if(!tile)
            {
                return null;
            }

            var localPosition = tile.transform.localPosition;
            var tileShift = localPosition * mapViewContext.Cut;
            Debug.Log(tileShift);
            var tileResolution = mapViewContext.TileResolution * converter.GetZoomMultiplier(zoomLevel);
            var pixelShift = new PixelLocation((int)(tileShift.x * tileResolution), -(int)(tileShift.z * tileResolution));
            var centerLocation = tile.Location - pixelShift;

            return centerLocation;
        }
    }
}