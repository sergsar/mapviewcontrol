using UnityEngine;
using System;

namespace MapViewScripts
{
    public class MapTile : MonoBehaviour
    {
        private float scale;
        private PixelLocation location;
        private TileLoaderCallback tileLoaderCallback = () => { };
        private MapLevelContext mapLevelContext;

        public TileLoaderCallback TileLoaderCallback { set { tileLoaderCallback = value; } }
        public MapLevelContext MapLevelContext { set { mapLevelContext = value; } }

        private void OnDestroy() //cleanup memory
        {
            Destroy(GetComponent<Renderer>().sharedMaterial.mainTexture);
            Destroy(GetComponent<Renderer>().sharedMaterial);
        }

        private float Limit(float value)
        {
            float result = value;
            if (Mathf.Abs(value) > (1F + scale) * 0.5F)
            {
                result = (value - 1F * Mathf.Sign(value));
            }
            return result;
        }

        public void Construct(float scale, PixelLocation location)
        {
            this.location = location;
            this.scale = scale;
            GetComponent<Renderer>().sharedMaterial = Instantiate<Material>(GetComponent<Renderer>().sharedMaterial);
        }

        public void Translate(Vector3 difference)
        {
            transform.Translate(difference);

            var localPosition = transform.localPosition;

            localPosition.x = Limit(localPosition.x);
            localPosition.z = Limit(localPosition.z);

            if(localPosition != transform.localPosition)
            {
                tileLoaderCallback();
            }

            transform.localPosition = localPosition;
        }
    }
}