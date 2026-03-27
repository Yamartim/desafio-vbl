using UnityEngine;
using System.Collections.Generic;
using System;

// Classes to model and parse the json data obtained through the API
[Serializable]
public class WeatherData
{
    public Status current_status;
    public List<PredictedStatus> predicted_status;

    public static WeatherData FromJson(string json)
    {
        Debug.Log($"Parsing JSON: {json}");
        try
        {
            var weatherData = JsonUtility.FromJson<WeatherData>(json);
            Debug.Log($"Parsed WeatherData: {weatherData}");
            return weatherData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse JSON: {e.Message}");
            return null;
        }        
    }

    public override string ToString()
    {
        return $"Current Status:\n" +
               $"Vehicle Density: {current_status.vehicleDensity:F4}\n" +
               $"Average Speed: {current_status.averageSpeed:F4}\n" +
               $"Weather: {current_status.weather}\n\n" +
               $"Predicted Statuses:\n" +
               $"{string.Join("\n---------------\n", predicted_status)}";
    }
}

[Serializable]
public class Status
{
    public double vehicleDensity;
    public double averageSpeed;
    public string weather;


    public WeatherCondition GetWeatherCondition()
    {
        return weather switch
        {
            "sunny" => WeatherCondition.Sunny,
            "clouded" => WeatherCondition.Clouded,
            "foggy" => WeatherCondition.Foggy,
            "light rain" => WeatherCondition.LightRain,
            "heavy rain" => WeatherCondition.HeavyRain,
            _ => WeatherCondition.Sunny
        };
    }

    public override string ToString()
    {
        return $"Vehicle Density: {vehicleDensity:F4}\n" +
               $"Average Speed: {averageSpeed:F4}\n" +
               $"Weather: {weather}";
    }
}

[Serializable]
public class PredictedStatus
{
    public int estimated_time;
    public Status predictions;

    public override string ToString()
    {
        return $"Estimated Time: {estimated_time}\n" +
               $"Predictions: {predictions}";
    }
}

public enum WeatherCondition
{
    Sunny,
    Clouded,
    Foggy,
    LightRain,
    HeavyRain
}