using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapPixelConverter
    {
        private static readonly float GOOGLEOFFSET = 268435456f;
        private static readonly float GOOGLEOFFSET_RADIUS = 85445659.44705395f;//GOOGLEOFFSET / Mathf.PI;

        private readonly float MATHPI_180 = Mathf.PI / 180f;

        private readonly float preLonToX1 = GOOGLEOFFSET_RADIUS * (Mathf.PI / 180f);

        public int LonToX(float lon)
        {
            return ((int)Mathf.Round(GOOGLEOFFSET + preLonToX1 * lon));
        }

        public int LatToZ(float lat)
        {
            return (int)Mathf.Round(GOOGLEOFFSET - GOOGLEOFFSET_RADIUS * Mathf.Log((1f + Mathf.Sin(lat * MATHPI_180)) / (1f - Mathf.Sin(lat * MATHPI_180))) / 2f);
        }

        public float XToLon(float x)
        {
            return ((Mathf.Round(x) - GOOGLEOFFSET) / GOOGLEOFFSET_RADIUS) * 180f / Mathf.PI;
        }

        public float ZToLat(float z)
        {
            return (Mathf.PI / 2f - 2f * Mathf.Atan(Mathf.Exp((Mathf.Round(z) - GOOGLEOFFSET) / GOOGLEOFFSET_RADIUS))) * 180f / Mathf.PI;
        }

        public float AdjustLonByPixels(float lon, int delta, int zoom)
        {
            return XToLon(LonToX(lon) + (delta << (21 - zoom)));
        }

        public float AdjustLatByPixels(float lat, int delta, int zoom)
        {
            return ZToLat(LatToZ(lat) + (delta << (21 - zoom)));
        }

        public int GetZoomMultiplier(int zoom)
        {
            return 1 << (21 - zoom);
        }

        //public float GetZoomMultiplierFloat(float zoom)
        //{
        //    return Mathf.Pow(2F, 21F - zoom);
        //}
    }
}