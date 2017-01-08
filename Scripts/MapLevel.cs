using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevel : MonoBehaviour
    {
        private SpownMapLevelCallback spownMapLevelCallback = (l, z, a) => { };
        private Action onDestroyCallback;
        private List<MapTile> tiles = new List<MapTile>();
        private MapTileUpdater mapTileUpdater;
        private UnityEngine.Object tileRefObject;
        private MapViewContext mapViewContext;
        private int zoomLevel;
        private PixelLocation initLocation;
        private bool half;
        private bool halfFlag;

        private const float fullSquareMagnitude = 3F;
        private const float halfSquareMagnitude = 0.75F;
        private const float quoterSquareMagnitude = 0.1875F;

        public SpownMapLevelCallback SpownMapLevelCallback { set { spownMapLevelCallback = value; } }
        public Action OnDestroyCallback {  set { onDestroyCallback = value; } }

        public MapTileUpdater MapTileUpdater { set { mapTileUpdater = value; } }
        public UnityEngine.Object TileRefObject { set { tileRefObject = value; } }

        public int ZoomLevel { get { return zoomLevel; } }

        public MapViewContext MapViewContext { set { mapViewContext = value; } }

        private void Update()
        {
            if(transform.localScale.sqrMagnitude < quoterSquareMagnitude || transform.localScale.sqrMagnitude > fullSquareMagnitude)
            {
                Destroy(gameObject);
            }

            half = transform.localScale.sqrMagnitude > halfSquareMagnitude;
            if (half && !halfFlag)
            {
                spownMapLevelCallback(initLocation, zoomLevel + 1, MapLevelAlign.Upper);
            }
            if (!half && halfFlag)
            {
                spownMapLevelCallback(initLocation, zoomLevel - 1, MapLevelAlign.Lower);
            }

            halfFlag = half;
        }

        private void OnDestroy()
        {
            onDestroyCallback();
        }

        public void Construct(PixelLocation initLocation, int zoomLevel)
        {
            this.zoomLevel = zoomLevel;
            this.initLocation = initLocation;
            ConstructInternal();
        }

        private void ConstructInternal()
        {
            var converter = new MapPixelConverter();
            var tileStep = mapViewContext.TileResolution * converter.GetZoomMultiplier(zoomLevel);
            var cut = mapViewContext.Cut;
            var startLocation = initLocation + new PixelLocation(-1, 1) * (int)(tileStep * 0.5F * (cut - 1));
            var levelStep = tileStep * cut;
            var tileScale = 1F / cut;
            Func<int, float> place = (index) => (tileScale - 1F) * 0.5F + tileScale * index;
            Func<int, int, int> locationPlace = (center, index) => (int)(center + tileStep * index);
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
            transform.localScale += Vector3.one * delta;
        }
    }
}