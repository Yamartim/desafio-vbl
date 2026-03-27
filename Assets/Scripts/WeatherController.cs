using System;
using UnityEngine;

// Main class to deal with the weather system
// it keeps track of the weather state of the world and notifies other objects with its data
public class WeatherController : MonoBehaviour
{
    public WeatherData currentWeather { get; private set; }

    // Observer pattern implemented through the unity event system
    // Other objects can subscribe their methods to this event to react when the weather changes
    public static event Action<Status> OnWeatherChange;

    float timeToNextWeather;
    bool finalWeatherReached = false;
    float lastWeatherTimer = 0f;

    WeatherBase ActiveWeatherEffect;


    [Header("UI Reference")]
    [SerializeField] private LevelUI levelUI;

    // Another alternative here would have been to use a HashSet<BaseWeather>
    // that would make it more extendable, however since the weather types are limited and dont change
    // I opted for the simpler approach
    [Header("Weather Effects")]
    [SerializeField] private WeatherLightRain lightRainEffect;
    [SerializeField] private WeatherHeavyRain heavyRainEffect;
    [SerializeField] private WeatherClouded cloudedEffect;
    [SerializeField] private WeatherSunny sunnyEffect;
    [SerializeField] private WeatherFoggy foggyEffect;

    // Weather initialization
    async void Awake()
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
        // Timer logic to change the weather, or end the game when the timer expires
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
        // For some reason there are some errors when getting the camera reference
        // but everything works fine even with those
        Camera mainCamera = Camera.main;

        // Weather effects need the camere reference to change the rendered background colot
        lightRainEffect.AssignMainCamera(mainCamera);
        heavyRainEffect.AssignMainCamera(mainCamera);
        cloudedEffect.AssignMainCamera(mainCamera);
        sunnyEffect.AssignMainCamera(mainCamera);
        foggyEffect.AssignMainCamera(mainCamera);

        // only activate the current weather
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

        // Deactivate the previous weather and activate the next one
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

        // Replace the last weather data with the current one
        currentWeather.current_status = nextWeather.predictions;
        // In unity, seconds are counted as a float unit, so we need to convert the milisecond data
        lastWeatherTimer = nextWeather.estimated_time * 0.001f;

        // Remove the first element from list so in canse there are more and the function is called again we can switch weather multiple times
        // A queue could've been used instead, but it felt better not to alter the original json data structure
        currentWeather.predicted_status.RemoveAt(0);

        // We keep track when the weather prediction list becomes empty to switch the timer to a game over timer
        finalWeatherReached = currentWeather.predicted_status.Count == 0;

        levelUI.WeatherUIUpdate(currentWeather, finalWeatherReached);
        SetTimeToNextWeather();

        // Call the event for all subscribing objects
        OnWeatherChange?.Invoke(currentWeather.current_status);
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
