﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapView : MonoBehaviour, IRoutineBehaviour
    {
        new private Collider collider;

        private MapLevelSpowner mapLevelSpowner;
        private float mapServiceWaitTime = 0.3F;
        private int tileResolution = 350;
        private int cut = 3;
        private float latitude = 55.75275F;
        private float longitude = 37.62074F;
        private MapLocation mapLocation = new MapLocation() { Longitude = 37.62074F, Latitude = 55.75275F };
        private int zoomLevel = 11;

        [SerializeField]
        private Object tileRefObject;

        private void Start()
        {
            var tileLoadingService = new TileLoadingService(this, mapServiceWaitTime);
            var mapViewContext = new MapViewContext(cut, tileResolution, tileLoadingService);

            var mapTileUpdater = new MapTileUpdater(mapViewContext);

            var converter = new MapPixelConverter();
            var pixelLocation = new PixelLocation() { X = converter.LonToX(longitude), Z = converter.LatToZ(latitude) };
            
            var mapLevelFactory = new MapLevelFactory(mapViewContext, mapTileUpdater, tileRefObject, gameObject);
            mapLevelSpowner = new MapLevelSpowner(mapLevelFactory);
            mapLevelSpowner.SpownMapLevel(pixelLocation, zoomLevel);

            

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

            mapLevelSpowner.ForEach(p => p.Translate(difference));
        }

        private void OnScrollWheel(Collider collider, InputMaster.WheelScrollArgs args)
        {
            mapLevelSpowner.ForEach(p => p.Scale(args.WheelDelta));
        }
    }
}