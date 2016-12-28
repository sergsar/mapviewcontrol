using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class MapTileApplier
    {
        public void Apply(GameObject gameObject, byte[] data, int textureResolution)
        {
            var sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            var oldTexture = sharedMaterial.mainTexture;
            var texture = new Texture2D(textureResolution, textureResolution);
            if (texture.LoadImage(data))
            {
                sharedMaterial.mainTexture = texture;
            }
            else
            {
                MonoBehaviour.Destroy(texture);
            }

            MonoBehaviour.Destroy(oldTexture);
        }

    }
}