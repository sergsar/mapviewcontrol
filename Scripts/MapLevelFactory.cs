using UnityEngine;

namespace MapViewScripts
{
    public class MapLevelFactory
    {
        private UnityEngine.Object tileRefObject;
        private MapContext mapContext;
        private TileUpdaterCallback tileUpdaterCallback;
        private GameObject parent;
        public MapLevelFactory(MapContext mapContext, TileUpdaterCallback tileUpdaterCallback, Object tileRefObject, GameObject parent)
        {
            this.tileRefObject = tileRefObject;
            this.mapContext = mapContext;
            this.tileUpdaterCallback = tileUpdaterCallback;
            this.parent = parent;
        }

        public ITranslatable GetMapLevel(PixelLocation pixelLocation, int zoomLevel)
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.MapContext = mapContext;
            mapLevel.TileUpdaterCallback = tileUpdaterCallback;
            mapLevel.TileRefObject = tileRefObject;

            mapLevel.gameObject.SetParent(parent);
            mapLevel.transform.localPosition = Vector3.zero;
            mapLevel.transform.localScale = Vector3.one;

            mapLevel.Construct(pixelLocation, zoomLevel);

            return mapLevel;
        }
    }
}