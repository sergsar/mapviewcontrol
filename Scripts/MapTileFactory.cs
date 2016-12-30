using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapTileFactory
    {
        private InstanciateCallback instanciateCallback;
        private MapLevelContext mapLevelContext;

        public MapTileFactory(InstanciateCallback instanciateCallback, MapLevelContext mapLevelContext)
        {
            this.instanciateCallback = instanciateCallback;
            this.mapLevelContext = mapLevelContext;
        }

        public MapTile GetMapTile()
        {
            var tile = (GameObject)instanciateCallback();

            var tileComponent = tile.AddComponent<MapTile>();
            tileComponent.MapLevelContext = mapLevelContext;

            return tileComponent;
        }
    }
}