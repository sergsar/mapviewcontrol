using UnityEngine;
using System;

namespace MapViewScripts
{
    public class MapTile : MonoBehaviour
    {
        private float scale;
        private PixelLocation location;
        private TileUpdaterCallback tileUpdaterCallback = (p) =>  null;
        private MapLevelContext mapLevelContext;

        public TileUpdaterCallback TileUpdaterCallback { set { tileUpdaterCallback = value; } }
        public MapLevelContext MapLevelContext { set { mapLevelContext = value; } }

        public PixelLocation Location { get { return location; } }

        public int ZoomLevel { get { return mapLevelContext.ZoomLevel; } } 

        private void OnDestroy() //cleanup memory
        {
            Destroy(GetComponent<Renderer>().sharedMaterial.mainTexture);
            Destroy(GetComponent<Renderer>().sharedMaterial);
        }

        private bool Limit(ref float value, out int locationDif)
        {
            locationDif = 0;
            bool limited = false;
            if (Mathf.Abs(value) > (1F + scale) * 0.5F)
            {
                value = (value - 1F * Mathf.Sign(value));
                limited = true;
                locationDif = (int)(mapLevelContext.Step * Mathf.Sign(value));
            }
            return limited;
        }

        private void UpdateTile()
        {
            StopCoroutine(tileUpdaterCallback(this));
            StartCoroutine(tileUpdaterCallback(this));
        }

        public void Construct(PixelLocation location)
        {
            this.location = location;
            scale = mapLevelContext.TileScale;
            GetComponent<Renderer>().sharedMaterial = Instantiate<Material>(GetComponent<Renderer>().sharedMaterial);
            UpdateTile();
        }

        public void Translate(Vector3 difference)
        {
            transform.Translate(difference);

            var localPosition = transform.localPosition;

            int locationDifX;
            int locationDifZ;

            var limitX = Limit(ref localPosition.x, out locationDifX);
            var limitZ = Limit(ref localPosition.z, out locationDifZ);

            location.X += locationDifX;
            location.Z += locationDifZ;


            if (limitX || limitZ)
            {
                UpdateTile();
            }

            transform.localPosition = localPosition;
        }
    }
}