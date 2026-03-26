using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentWeatherUI;
    [SerializeField] private TMP_Text nextWeatherUI;
    [SerializeField] private TMP_Text nextWeatherTimerUI;

    [SerializeField] private TMP_Text scoreBoard;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreBoard.SetText($"Score: {GameManager.instance.CurrentScore}");
    }


    public void WeatherUIUpdate(WeatherData currentWeather, bool finalWeatherReached)
    {
        if (currentWeather is null)
        {
            currentWeatherUI.text = "Failed to fetch weather data.";
            nextWeatherUI.text = "";
            nextWeatherTimerUI.text = "";
            return;
        }

        currentWeatherUI.text = currentWeather.current_status.ToString();

        if (!finalWeatherReached)
        {
            Status nextStatus = currentWeather.predicted_status[0].predictions;
            nextWeatherUI.text = nextStatus.ToString();

        }
        else
        {
            nextWeatherUI.color = Color.red;
            nextWeatherUI.text = "None!\nHurry to the end of the level\nor it's game over!";
        }

    }

    public void TimerUIUpdate(float timeToNextWeather, bool finalWeatherReached)
    {
        
        string minutes = ((int)timeToNextWeather / 60).ToString("D2");
        string seconds = ((int)timeToNextWeather % 60).ToString("D2");
        nextWeatherTimerUI.text = $"{minutes}:{seconds}";

        if (finalWeatherReached)
        {
            nextWeatherTimerUI.color = Color.red;
        }
    }
}
