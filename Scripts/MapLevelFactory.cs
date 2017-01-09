using UnityEngine;

namespace MapViewScripts
{
    public class MapLevelFactory
    {
        private UnityEngine.Object tileRefObject;
        private MapViewContext mapViewContext;
        private MapTileUpdater mapTileUpdater;
        public MapLevelFactory(MapViewContext mapViewContext, MapTileUpdater mapTileUpdater, Object tileRefObject)
        {
            this.tileRefObject = tileRefObject;
            this.mapViewContext = mapViewContext;
            this.mapTileUpdater = mapTileUpdater;
        }

        public MapLevel GetMapLevel(PixelLocation pixelLocation, int zoomLevel)
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.MapViewContext = mapViewContext;
            mapLevel.MapTileUpdater = mapTileUpdater;
            mapLevel.TileRefObject = tileRefObject;

            mapLevel.Init(pixelLocation, zoomLevel);
            mapLevel.Construct();

            return mapLevel;
        }
    }
}