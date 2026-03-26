using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text CurrentWeatherUI;
    [SerializeField] private TMP_Text NextWeatherUI;
    [SerializeField] private TMP_Text NextWeatherTimerUI;

    [SerializeField] private TMP_Text ScoreBoard;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreBoard.SetText($"Score: {GameManager.instance.CurrentScore}");
    }


    public void WeatherUIUpdate(WeatherData currentWeather, bool finalWeatherReached)
    {
        if (currentWeather is null)
        {
            CurrentWeatherUI.text = "Failed to fetch weather data.";
            NextWeatherUI.text = "";
            NextWeatherTimerUI.text = "";
            return;
        }

        CurrentWeatherUI.text = currentWeather.current_status.ToString();

        if (!finalWeatherReached)
        {
            Status nextStatus = currentWeather.predicted_status[0].predictions;
            NextWeatherUI.text = nextStatus.ToString();

        }
        else
        {
            NextWeatherUI.color = Color.red;
            NextWeatherUI.text = "None!\nHurry to the end of the level\nor it's game over!";
        }

    }

    public void TimerUIUpdate(float timeToNextWeather, bool finalWeatherReached)
    {
        
        string minutes = ((int)timeToNextWeather / 60).ToString("D2");
        string seconds = ((int)timeToNextWeather % 60).ToString("D2");
        NextWeatherTimerUI.text = $"{minutes}:{seconds}";

        if (finalWeatherReached)
        {
            NextWeatherTimerUI.color = Color.red;
        }
    }
}
