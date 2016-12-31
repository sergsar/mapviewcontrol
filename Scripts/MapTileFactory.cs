using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapTileFactory
    {
        private InstanciateCallback instanciateCallback;
        private MapLevelContext mapLevelContext;
        private TileUpdaterCallback tileUpdaterCallback;

        public MapTileFactory(InstanciateCallback instanciateCallback, MapLevelContext mapLevelContext, TileUpdaterCallback tileUpdaterCallback)
        {
            this.instanciateCallback = instanciateCallback;
            this.mapLevelContext = mapLevelContext;
            this.tileUpdaterCallback = tileUpdaterCallback;
        }

        public MapTile GetMapTile()
        {
            var tile = (GameObject)instanciateCallback();

            var tileComponent = tile.AddComponent<MapTile>();
            tileComponent.MapLevelContext = mapLevelContext;
            tileComponent.TileUpdaterCallback = tileUpdaterCallback;

            return tileComponent;
        }
    }
}