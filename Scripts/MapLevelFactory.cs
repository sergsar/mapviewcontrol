﻿using UnityEngine;

namespace MapViewScripts
{
    public class MapLevelFactory
    {
        private UnityEngine.Object tileRefObject;
        private MapContext mapContext;
        private MapTileUpdater mapTileLoader;
        public MapLevelFactory(MapContext mapContext, MapTileUpdater mapTileLoader, Object tileRefObject)
        {
            this.tileRefObject = tileRefObject;
            this.mapContext = mapContext;
            this.mapTileLoader = mapTileLoader;
        }

        public MapLevel GetMapLevel()
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.MapContext = mapContext;
            mapLevel.MapTileLoader = mapTileLoader;
            mapLevel.TileRefObject = tileRefObject;

            return mapLevel;
        }
    }
}