using UnityEngine;

namespace MapViewScripts
{
    public class MapLevelFactory
    {
        private UnityEngine.Object tileRefObject;
        private MapViewContext mapViewContext;
        private MapTileUpdater mapTileUpdater;
        private GameObject parent;

        public MapLevelFactory(MapViewContext mapViewContext, MapTileUpdater mapTileUpdater, Object tileRefObject, GameObject parent)
        {
            this.tileRefObject = tileRefObject;
            this.mapViewContext = mapViewContext;
            this.mapTileUpdater = mapTileUpdater;
            this.parent = parent;
        }

        public MapLevel GetMapLevelLower(PixelLocation pixelLocation, int zoomLevel)
        {
            return GetMapLevelInternal(pixelLocation, zoomLevel);
        }

        public MapLevel GetMapLevelUpper(PixelLocation pixelLocation, int zoomLevel)
        {
            var mapLevel = GetMapLevelInternal(pixelLocation, zoomLevel);
            mapLevel.transform.localScale *= 0.25F;
            return mapLevel;
        }

        public MapLevel GetMapLevelInternal(PixelLocation pixelLocation, int zoomLevel)
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.MapViewContext = mapViewContext;
            mapLevel.MapTileUpdater = mapTileUpdater;
            mapLevel.TileRefObject = tileRefObject;

            mapLevel.gameObject.SetParent(parent);
            mapLevel.transform.localPosition = Vector3.zero;
            mapLevel.transform.localScale = Vector3.one;

            mapLevel.Construct(pixelLocation, zoomLevel);

            return mapLevel;
        }
    }
}