using UnityEngine;
using System.Collections;

namespace MapViewScripts
{
    public class YandexMapRequest : StaticMapRequest
    {
        protected override string startUrl { get { return "https://static-maps.yandex.ru/1.x/"; } }
        protected override string apiName { get { return "Yandex"; } }

        public YandexMapRequest(float latitude, float longitude, int zoomLevel, int resolution) : base (latitude, longitude, zoomLevel, resolution) { }
        public override string GetUrl()
        {
            var qs = "";
            qs += "ll=" + WWW.UnEscapeURL(string.Format("{0},{1}", longitude, latitude));
            qs += "&z=" + zoomLevel.ToString();
            qs += "&size=" + WWW.UnEscapeURL(string.Format("{0},{0}", resolution));
            qs += "&l=sat";


            var url = startUrl + "?" + qs;

            return url;
        }
    }
}