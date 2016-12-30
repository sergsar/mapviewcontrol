using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapLevelContext
    {
        public int ZoomLevel { get; private set; }
        public float Step { get; private set; }
        public float TileScale { get; private set; }

        public MapLevelContext(int zoomLevel, float step, float tileScale)
        {
            ZoomLevel = zoomLevel;
            Step = step;
            TileScale = tileScale;
        }
    }
}