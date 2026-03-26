using UnityEngine;

public abstract class WeatherBase : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Color SkyColor;

    void OnEnable()
    {
        ApplyWeatherEffects();
    }

    void OnDisable()
    {
        RemoveWeatherEffects();
    }

    virtual protected void ApplyWeatherEffects()
    {
        mainCamera.backgroundColor = SkyColor;
    }

    virtual protected void RemoveWeatherEffects()
    {
        mainCamera.backgroundColor = Color.white;
    }

    public void AssignMainCamera(Camera cam)
    {
        mainCamera = cam;
    }
}
