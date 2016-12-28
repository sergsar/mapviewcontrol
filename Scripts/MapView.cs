using UnityEngine;
using System.Collections.Generic;

namespace MapViewScripts
{
    public class MapView : MonoBehaviour
    {
        new private Collider collider;

        private List<MapLevel> mapLevels = new List<MapLevel>();
        private int cut = 3;

        private float latitude = 55.75275F;
        private float longitude = 37.62074F;
        private int zoomLevel = 11;

        [SerializeField]
        private Object tileObject;

        private void Start()
        {
            var mapLevel = new GameObject("MapLevel").AddComponent<MapLevel>();
            mapLevel.gameObject.SetParent(gameObject);
            mapLevel.transform.localPosition = Vector3.zero;
            mapLevel.Construct(tileObject, cut);
            mapLevels.Add(mapLevel);

            collider = GetComponent<Collider>();
            InputMaster.Instance.AddPointDragEventHandler(collider, OnPointerDrag);
        }

        private void OnDestroy()
        {
            InputMaster.Instance.RemovePointDragHandler(collider);
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
    }
}