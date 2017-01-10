using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapView : MonoBehaviour, IRoutineBehaviour
    {
        new private Collider collider;

        private MapLevelFactory mapLevelFactory;
        private List<MapLevel> mapLevels = new List<MapLevel>();
        private float mapServiceWaitTime = 0.3F;
        private int tileResolution = 350;
        private int cut = 3;
        private float latitude = 55.75275F;
        private float longitude = 37.62074F;
        private MapLocation mapLocation = new MapLocation() { Longitude = 37.62074F, Latitude = 55.75275F };
        private int initialZoomLevel = 11;
        private int zoomLevel;
        private float zoomLevelFloat;
        private MapPixelConverter converter = new MapPixelConverter();

        [SerializeField]
        private Object tileRefObject;

        private void Start()
        {
            zoomLevelFloat = zoomLevel = initialZoomLevel;

            var tileLoadingService = new TileLoadingService(this, mapServiceWaitTime);
            var mapViewContext = new MapViewContext(cut, tileResolution, tileLoadingService);

            var mapTileUpdater = new MapTileUpdater(mapViewContext);

            var pixelLocation = new PixelLocation() { X = converter.LonToX(longitude), Z = converter.LatToZ(latitude) };
            
            mapLevelFactory = new MapLevelFactory(mapViewContext, mapTileUpdater, tileRefObject);

            var mapLevel = mapLevelFactory.GetMapLevel(pixelLocation, zoomLevel);
            mapLevels.Add(mapLevel);

            mapLevel.gameObject.SetParent(gameObject);
            mapLevel.transform.localPosition = Vector3.zero;
            mapLevel.transform.localScale = Vector3.one;

            collider = GetComponent<Collider>();
            InputMaster.Instance.AddPointDragEventHandler(collider, OnPointerDrag);
            InputMaster.Instance.AddWheelScrollEventHandler(collider, OnScrollWheel);
        }

        private void OnDestroy()
        {
            InputMaster.Instance.RemovePointDragHandler(collider);
            InputMaster.Instance.RemoveWheelScrollEventHandler(collider);
        }

        private void OnPointerDrag(Collider collider, InputMaster.PointerDragArgs args)
        {
            var plane = new Plane(transform.up, transform.position);

            var ray1 = InputMaster.Instance.CurrentCamera.ScreenPointToRay(args.ScreenPosition);
            var ray2 = InputMaster.Instance.CurrentCamera.ScreenPointToRay(args.ScreenPosition + args.ScreenPositionDelta);

            var raycastEnter1 = default(float);
            var raycastEnter2 = default(float);

            var cast1 = plane.Raycast(ray1, out raycastEnter1);
            var cast2 = plane.Raycast(ray2, out raycastEnter2);

            if (!cast1 || !cast2)
            {
                return;
            }

            var hitPoint1 = ray1.GetPoint(raycastEnter1);
            var hitPoint2 = ray2.GetPoint(raycastEnter2);

            var difference = hitPoint2 - hitPoint1;

            //Debug.LogFormat("hitPoint1 {0}, hitPoint2 {1}, difference {2}", hitPoint1, hitPoint2, difference);

            mapLevels.ForEach(p => p.Translate(difference));
        }

        private void OnScrollWheel(Collider collider, InputMaster.WheelScrollArgs args)
        {
            var wheelDelta = args.WheelDelta;
            zoomLevelFloat += wheelDelta;
            var intZoomLevelDelta = (int)zoomLevelFloat;
            if (intZoomLevelDelta != zoomLevel)
            {
                var destroyLevels = new List<MapLevel>();
                var factorUpdatePow = 0F;
                if(intZoomLevelDelta < zoomLevel)
                {
                    destroyLevels = mapLevels.Where(p => p.ZoomLevel > zoomLevel).ToList();
                    factorUpdatePow = -1F;
                }
                else
                {
                    destroyLevels = mapLevels.Where(p => p.ZoomLevel < zoomLevel).ToList();
                    factorUpdatePow = 1F;
                }
                mapLevels.RemoveAll(p => destroyLevels.Contains(p));
                destroyLevels.ForEach(p => Destroy(p.gameObject));
                mapLevels.ForEach(p => p.UpdateScaleFactor(factorUpdatePow));

                if (!mapLevels.Any(p => p.ZoomLevel == intZoomLevelDelta))
                {
                    var pixelLocation = new PixelLocation() { X = converter.LonToX(longitude), Z = converter.LatToZ(latitude) };
                    var mapLevel = mapLevelFactory.GetMapLevel(pixelLocation, intZoomLevelDelta);
                    mapLevel.gameObject.SetParent(gameObject);
                    mapLevel.transform.localPosition = Vector3.zero;
                    if (intZoomLevelDelta < zoomLevel)
                    {
                        mapLevel.transform.localScale = Vector3.one * 2F;
                    }
                    else
                    {
                        mapLevel.transform.localScale = Vector3.one * 0.5F;
                    }

                    mapLevels.Add(mapLevel);
                }

                //Debug.LogFormat("intZoomLevelDelta {0} : zoomLevel {1} : zoomLevelDelta {2}", intZoomLevelDelta, zoomLevel, zoomLevelFloat);
            }
            zoomLevel = intZoomLevelDelta;

            mapLevels.ForEach(p => p.Scale(zoomLevelFloat % 1F));
        }
    }
}