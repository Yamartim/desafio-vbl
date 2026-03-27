using UnityEngine;

// This is the only weather type that needed overriding due to using the fog system
public class WeatherFoggy : WeatherBase
{
    override protected void ApplyWeatherEffects()
    {
        base.ApplyWeatherEffects();
        RenderSettings.fog = true;
    } 

    override protected void RemoveWeatherEffects()
    {
        base.RemoveWeatherEffects();
        RenderSettings.fog = false;
    }
}
