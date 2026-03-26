using UnityEngine;

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
