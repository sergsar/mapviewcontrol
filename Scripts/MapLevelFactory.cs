using UnityEngine;

namespace MapViewScripts
{
    public class MapLevelFactory
    {
        private UnityEngine.Object tileRefObject;
        private MapContext mapContext;
        private TileUpdaterCallback tileUpdaterCallback;
        public MapLevelFactory(MapContext mapContext, TileUpdaterCallback tileUpdaterCallback, Object tileRefObject)
        {
            this.tileRefObject = tileRefObject;
            this.mapContext = mapContext;
            this.tileUpdaterCallback = tileUpdaterCallback;
        }

        public MapLevel GetMapLevel()
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.MapContext = mapContext;
            mapLevel.TileUpdaterCallback = tileUpdaterCallback;
            mapLevel.TileRefObject = tileRefObject;

            return mapLevel;
        }
    }
}