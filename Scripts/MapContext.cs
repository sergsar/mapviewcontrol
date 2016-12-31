using UnityEngine;
namespace MapViewScripts
{
    public class MapContext
    {
        public int Cut { get; private set; }
        public int TileResolution { get; private set; }

        public TileLoadingService MapService { get; private set; }

        public MapContext(int cut, int tileResolution, TileLoadingService mapService)
        {
            Cut = cut;
            TileResolution = tileResolution;
            MapService = mapService;
        }
    }
}