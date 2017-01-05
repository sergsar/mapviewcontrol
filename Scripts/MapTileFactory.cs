using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapTileFactory
    {
        private MapLevelContext mapLevelContext;
        private MapTileUpdater mapTileUpdater;
        private Object tileRefObject;

        public MapTileFactory(Object tileRefObject, MapLevelContext mapLevelContext, MapTileUpdater mapTileUpdater)
        {
            this.tileRefObject = tileRefObject;
            this.mapLevelContext = mapLevelContext;
            this.mapTileUpdater = mapTileUpdater;
        }

        public MapTile GetMapTile()
        {
            var tile = (GameObject)MonoBehaviour.Instantiate(tileRefObject);

            var tileComponent = tile.AddComponent<MapTile>();
            tileComponent.MapLevelContext = mapLevelContext;
            tileComponent.MapTileUpdater = mapTileUpdater;

            return tileComponent;
        }
    }
}