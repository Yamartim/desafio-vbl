using System;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public WeatherData currentWeather { get; private set; }
    public static event Action<WeatherCondition> OnWeatherChange;

    float timeToNextWeather;
    bool finalWeatherReached = false;
    float lastWeatherTimer = 0f;

    WeatherBase ActiveWeatherEffect;


    [Header("UI Reference")]
    [SerializeField] private LevelUI levelUI;

    [Header("Weather Effects")]
    [SerializeField] private WeatherLightRain lightRainEffect;
    [SerializeField] private WeatherHeavyRain heavyRainEffect;
    [SerializeField] private WeatherClouded cloudedEffect;
    [SerializeField] private WeatherSunny sunnyEffect;
    [SerializeField] private WeatherFoggy foggyEffect;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        try
        {
            RequestResult result = await ApiRequester.GetRequest();
            if (result.Success)
            {
                currentWeather = result.WeatherData;
                WeatherCondition condition = result.WeatherData.current_status.GetWeatherCondition();

                switch (condition)
                {
                    case WeatherCondition.Sunny:
                        ActiveWeatherEffect = sunnyEffect;
                        break;
                    case WeatherCondition.LightRain:
                        ActiveWeatherEffect = lightRainEffect;
                        break;
                    case WeatherCondition.HeavyRain:
                        ActiveWeatherEffect = heavyRainEffect;
                        break;
                    case WeatherCondition.Clouded:
                        ActiveWeatherEffect = cloudedEffect;
                        break;
                    case WeatherCondition.Foggy:
                        ActiveWeatherEffect = foggyEffect;
                        break;
                }
            } else
            {
                Debug.LogError($"Failed to fetch weather data: {result.HttpResult}");
                throw new Exception($"Failed to fetch weather data: {result.HttpResult}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to fetch weather data: {e.Message}");
        }

        SetupWeatherEffects();
        levelUI.WeatherUIUpdate(currentWeather, finalWeatherReached);
        SetTimeToNextWeather();
    }

    // Update is called once per frame
    async void Update()
    {
        if (timeToNextWeather > 0)
        {
            timeToNextWeather -= Time.deltaTime;
            levelUI.TimerUIUpdate(timeToNextWeather, finalWeatherReached);
            if (timeToNextWeather <= 0)
            {
                if (!finalWeatherReached)
                {
                    WeatherChange();
                }
                else
                {
                    await GameManager.instance.GameOver();
                }
            }
        }
    }

#region Weather Logic

    void SetupWeatherEffects()
    {
        Camera mainCamera = Camera.main;

        lightRainEffect.AssignMainCamera(mainCamera);
        heavyRainEffect.AssignMainCamera(mainCamera);
        cloudedEffect.AssignMainCamera(mainCamera);
        sunnyEffect.AssignMainCamera(mainCamera);
        foggyEffect.AssignMainCamera(mainCamera);

        lightRainEffect.gameObject.SetActive(false);
        heavyRainEffect.gameObject.SetActive(false);
        cloudedEffect.gameObject.SetActive(false);
        sunnyEffect.gameObject.SetActive(false);
        foggyEffect.gameObject.SetActive(false);

        ActiveWeatherEffect.gameObject.SetActive(true);
    }

    void WeatherChange()
    {
        if (currentWeather is null)
        {
            return;
        }

        PredictedStatus nextWeather = currentWeather.predicted_status[0];
        WeatherCondition nextCondition = nextWeather.predictions.GetWeatherCondition();

        ActiveWeatherEffect.gameObject.SetActive(false);
        switch (nextCondition)
        {
            case WeatherCondition.Sunny:
                ActiveWeatherEffect = sunnyEffect;
                break;
            case WeatherCondition.LightRain:
                ActiveWeatherEffect = lightRainEffect;
                break;
            case WeatherCondition.HeavyRain:
                ActiveWeatherEffect = heavyRainEffect;
                break;
            case WeatherCondition.Clouded:
                ActiveWeatherEffect = cloudedEffect;
                break;
            case WeatherCondition.Foggy:
                ActiveWeatherEffect = foggyEffect;
                break;
        }
        ActiveWeatherEffect.gameObject.SetActive(true);

        currentWeather.current_status = nextWeather.predictions;
        lastWeatherTimer = nextWeather.estimated_time * 0.001f;

        currentWeather.predicted_status.RemoveAt(0);

        finalWeatherReached = currentWeather.predicted_status.Count == 0;

        levelUI.WeatherUIUpdate(currentWeather, finalWeatherReached);
        SetTimeToNextWeather();

        OnWeatherChange?.Invoke(currentWeather.current_status.GetWeatherCondition());
    }

    private void SetTimeToNextWeather()
    {
        timeToNextWeather = !finalWeatherReached
            ? currentWeather.predicted_status[0].estimated_time * 0.001f
            : lastWeatherTimer;
        levelUI.TimerUIUpdate(timeToNextWeather, finalWeatherReached);
    }
#endregion

}
