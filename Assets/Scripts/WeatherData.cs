using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class WeatherData
{
    public Status current_status;

    public List<PredictedStatus> predicted_status;

    public WeatherCondition GetWeatherCondition()
    {
        return current_status.weather switch
        {
            "Sunny" => WeatherCondition.Sunny,
            "Clouded" => WeatherCondition.Clouded,
            "Foggy" => WeatherCondition.Foggy,
            "LightRain" => WeatherCondition.LightRain,
            "HeavyRain" => WeatherCondition.HeavyRain,
            _ => WeatherCondition.Sunny
        };
    }

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
               $"Vehicle Density: {current_status.vehicleDensity}\n" +
               $"Average Speed: {current_status.averageSpeed}\n" +
               $"Weather: {current_status.weather}\n\n" +
               $"Predicted Statuses:\n" +
               $"{string.Join("\n", predicted_status)}";
    }
}

[Serializable]
public class Status
{
    public double vehicleDensity;

    public double averageSpeed;

    public string weather;

    public override string ToString()
    {
        return $"Vehicle Density: {vehicleDensity}, Average Speed: {averageSpeed}, Weather: {weather}";
    }
}

[Serializable]
public class PredictedStatus
{
    public int estimated_time;

    public Status predictions;

    public override string ToString()
    {
        return $"Estimated Time: {estimated_time}, Predictions: {predictions}";
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