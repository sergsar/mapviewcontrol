using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapLevelSpowner
    {
        private MapLevelFactory mapLevelFactory;
        private List<MapLevel> mapLevels = new List<MapLevel>();
        public MapLevelSpowner(MapLevelFactory mapLevelFactory)
        {
            this.mapLevelFactory = mapLevelFactory;
        }

        public void SpownMapLevel(PixelLocation pixelLocation, int zoomLevel)
        {
            SpownMapLevel(pixelLocation, zoomLevel, MapLevelAlign.Lower);
        }

        public void SpownMapLevel(PixelLocation pixelLocation, int zoomLevel, MapLevelAlign mapLevelAlign)
        {
            if (zoomLevel <= 0 || zoomLevel >= 21 || mapLevels.Any(p => p.ZoomLevel == zoomLevel))
            {
                return;
            }
            MapLevel mapLevel = null;
            switch(mapLevelAlign)
            {
                case MapLevelAlign.Lower:
                    mapLevel = mapLevelFactory.GetMapLevelLower(pixelLocation, zoomLevel);
                    break;
                case MapLevelAlign.Upper:
                    mapLevel = mapLevelFactory.GetMapLevelUpper(pixelLocation, zoomLevel);
                    break;
                default:
                    break;
            }
            mapLevel.SpownMapLevelCallback = (l, z, a) => SpownMapLevel(l, z, a);
            mapLevel.OnDestroyCallback = () => mapLevels.Remove(mapLevel);
            mapLevels.Add(mapLevel);
        }

        public void ForEach(Action<MapLevel> action)
        {
            mapLevels.ForEach(p => action(p));
        }
    }
}