using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapTileFactory
    {
        private System.Func<Object> instanciator;
        private MapLevelContext mapLevelContext;

        public MapTileFactory(System.Func<Object> instanciator, MapLevelContext mapLevelContext)
        {
            this.instanciator = instanciator;
            this.mapLevelContext = mapLevelContext;
        }

        public MapTile GetMapTile()
        {
            var tile = (GameObject)instanciator();

            var tileComponent = tile.AddComponent<MapTile>();
            tileComponent.MapLevelContext = mapLevelContext;

            return tileComponent;
        }
    }
}