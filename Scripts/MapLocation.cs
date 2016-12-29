namespace MapViewScripts
{
    public class MapLocation
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public MapLocation() { }

        public MapLocation(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}