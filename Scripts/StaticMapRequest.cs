using UnityEngine;
using System.Collections;
using System;

namespace MapViewScripts
{

    public abstract class StaticMapRequest
    {
        protected abstract string apiName { get; }
        protected abstract string startUrl { get; }
        protected float latitude;
        protected float longitude;
        protected int zoomLevel;
        protected int resolution;

        public int Resolution { get { return resolution; } }
        public string Unique
        {
            get
            {
                return string.Format("{0}-{1}-{2}-z{3}-r{4}", apiName, latitude.ToString(), longitude.ToString(), zoomLevel.ToString(), resolution.ToString());
            }
        }

        public StaticMapRequest(float latitude, float longitude, int zoomLevel, int resolution)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.zoomLevel = zoomLevel;
            this.resolution = resolution;
        }
        public abstract string GetUrl();
    }
}