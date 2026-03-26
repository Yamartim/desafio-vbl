using System;
using UnityEngine;
using TMPro;

public class WeatherController : MonoBehaviour
{
    public WeatherData CurrentWeather { get; private set; }
    public static event Action<Status> OnWeatherChange;

    float timeToNextWeather;
    bool finalWeatherReached = false;
    float lastWeatherTimer = 0f;

    WeatherBase ActiveWeatherEffect;


    [Header("UI References")]
    [SerializeField] private TMP_Text CurrentWeatherUI;
    [SerializeField] private TMP_Text NextWeatherUI;
    [SerializeField] private TMP_Text NextWeatherTimerUI;

    [Header("Weather Effects")]
    [SerializeField] private WeatherLightRain LightRainEffect;
    [SerializeField] private WeatherHeavyRain HeavyRainEffect;
    [SerializeField] private WeatherClouded CloudedEffect;
    [SerializeField] private WeatherSunny SunnyEffect;
    [SerializeField] private WeatherFoggy FoggyEffect;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        try
        {
            RequestResult result = await ApiRequester.GetRequest();
            if (result.Success)
            {
                CurrentWeather = result.WeatherData;
                WeatherCondition condition = result.WeatherData.current_status.GetWeatherCondition();

                switch (condition)
                {
                    case WeatherCondition.Sunny:
                        ActiveWeatherEffect = SunnyEffect;
                        break;
                    case WeatherCondition.LightRain:
                        ActiveWeatherEffect = LightRainEffect;
                        break;
                    case WeatherCondition.HeavyRain:
                        ActiveWeatherEffect = HeavyRainEffect;
                        break;
                    case WeatherCondition.Clouded:
                        ActiveWeatherEffect = CloudedEffect;
                        break;
                    case WeatherCondition.Foggy:
                        ActiveWeatherEffect = FoggyEffect;
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
        WeatherUIUpdate();
        SetTimeToNextWeather();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToNextWeather > 0)
        {
            timeToNextWeather -= Time.deltaTime;
            TimerUIUpdate();
            if (timeToNextWeather <= 0)
            {
                if (!finalWeatherReached)
                {
                    WeatherChange();
                }
                else
                {
                    GameManager.instance.GameOver();
                }
            }
        }
    }

#region Weather Logic
    void WeatherChange()
    {
        if (CurrentWeather is null)
        {
            return;
        }

        PredictedStatus nextWeather = CurrentWeather.predicted_status[0];
        WeatherCondition nextCondition = nextWeather.predictions.GetWeatherCondition();

        ActiveWeatherEffect.gameObject.SetActive(false);
        switch (nextCondition)
        {
            case WeatherCondition.Sunny:
                ActiveWeatherEffect = SunnyEffect;
                break;
            case WeatherCondition.LightRain:
                ActiveWeatherEffect = LightRainEffect;
                break;
            case WeatherCondition.HeavyRain:
                ActiveWeatherEffect = HeavyRainEffect;
                break;
            case WeatherCondition.Clouded:
                ActiveWeatherEffect = CloudedEffect;
                break;
            case WeatherCondition.Foggy:
                ActiveWeatherEffect = FoggyEffect;
                break;
        }
        ActiveWeatherEffect.gameObject.SetActive(true);

        CurrentWeather.current_status = nextWeather.predictions;
        lastWeatherTimer = nextWeather.estimated_time * 0.001f;

        CurrentWeather.predicted_status.RemoveAt(0);

        finalWeatherReached = CurrentWeather.predicted_status.Count == 0;

        WeatherUIUpdate();
        SetTimeToNextWeather();

        OnWeatherChange?.Invoke(CurrentWeather.current_status);
    }

    private void SetTimeToNextWeather()
    {
        timeToNextWeather = !finalWeatherReached
            ? 10f//CurrentWeather.predicted_status[0].estimated_time * 0.001f
            : lastWeatherTimer;
        TimerUIUpdate();
    }
#endregion

#region UI Update Methods
    void WeatherUIUpdate()
    {
        if (CurrentWeather is null)
        {
            CurrentWeatherUI.text = "Failed to fetch weather data.";
            NextWeatherUI.text = "";
            NextWeatherTimerUI.text = "";
            return;
        }

        CurrentWeatherUI.text = CurrentWeather.current_status.ToString();

        if (!finalWeatherReached)
        {
            Status nextStatus = CurrentWeather.predicted_status[0].predictions;
            NextWeatherUI.text = nextStatus.ToString();

        }
        else
        {
            NextWeatherUI.color = Color.red;
            NextWeatherUI.text = "None!\nHurry to the end of the level\nor it's game over!";
        }

        TimerUIUpdate();
    }

    private void TimerUIUpdate()
    {
        
        string minutes = ((int)timeToNextWeather / 60).ToString("D2");
        string seconds = ((int)timeToNextWeather % 60).ToString("D2");
        NextWeatherTimerUI.text = $"{minutes}:{seconds}";

        if (finalWeatherReached)
        {
            NextWeatherTimerUI.color = Color.red;
        }
    }
#endregion

    void SetupWeatherEffects()
    {
        Camera mainCamera = Camera.main;

        LightRainEffect.AssignMainCamera(mainCamera);
        HeavyRainEffect.AssignMainCamera(mainCamera);
        CloudedEffect.AssignMainCamera(mainCamera);
        SunnyEffect.AssignMainCamera(mainCamera);
        FoggyEffect.AssignMainCamera(mainCamera);

        LightRainEffect.gameObject.SetActive(false);
        HeavyRainEffect.gameObject.SetActive(false);
        CloudedEffect.gameObject.SetActive(false);
        SunnyEffect.gameObject.SetActive(false);
        FoggyEffect.gameObject.SetActive(false);

        ActiveWeatherEffect.gameObject.SetActive(true);
    }
}
