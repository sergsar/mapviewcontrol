using System.Collections;
namespace MapViewScripts
{
    public class MapTileUpdater
    {
        private MapViewContext mapContext;

        public MapTileUpdater(MapViewContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public IEnumerator UpdateTile(MapTile tile)
        {
            var converter = new MapPixelConverter();
            var longitude = converter.XToLon(tile.Location.X);
            var latitude = converter.ZToLat(tile.Location.Z);
            var resolution = mapContext.TileResolution;

            var mapRequest = new YandexMapRequest(latitude, longitude, tile.ZoomLevel, resolution);
            var loadingService = mapContext.MapService;
            IEnumerator result = null;
            yield return result = loadingService.Load(mapRequest.GetUrl());
            var data = result.Current as byte[];
            if (tile != null && tile.gameObject && data != null)
            {
                var applier = new MapTileApplier();
                applier.Apply(tile.gameObject, data, resolution);
            }
        }
    }
}