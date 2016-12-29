using UnityEngine;
namespace MapViewScripts
{
    public class MapContext
    {
        public int Cut { get; private set; }
        public int TileResolution { get; private set; }

        public MapContext(int cut, int tileResolution)
        {
            Cut = cut;
            TileResolution = tileResolution;
        }
    }
}