using UnityEngine;
using iDVP.Platform.Entity;

public class MapViewControlScript : ControlElementView<MapViewControl>
{
    private bool initialized;
    private bool onSetupGone;

    private void Start()
    {
        Initialize();

        initialized = true;
        if (onSetupGone)
        {
            OnSetupView(Element);
        }
    }

    private void Initialize()
    {
        var mapViewGameObject = Instantiate(Resources.Load<GameObject>("MapView"));
        mapViewGameObject.SetParent(gameObject);
        mapViewGameObject.transform.localPosition = ActualSize * 0.5F;
        mapViewGameObject.transform.localScale = ActualSize;
    }

    private void Setup()
    {

    }

    protected override void OnSetupView(MapViewControl element)
    {
        onSetupGone = true;
        if (!initialized)
        {
            return;
        }

        base.OnSetupView(element);

        Setup();
    }
}
