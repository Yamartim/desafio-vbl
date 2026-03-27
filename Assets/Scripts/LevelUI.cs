using TMPro;
using UnityEngine;

// UI class for displaying API and gamplay information during the game
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
            currentWeatherUI.SetText("Failed to fetch weather data.");
            nextWeatherUI.SetText("");
            nextWeatherTimerUI.SetText("");
            return;
        }

        currentWeatherUI.SetText(currentWeather.current_status.ToString());

        if (!finalWeatherReached)
        {
            Status nextStatus = currentWeather.predicted_status[0].predictions;
            nextWeatherUI.SetText(nextStatus.ToString());

        }
        else
        {
            nextWeatherUI.color = Color.red;
            nextWeatherUI.SetText("None!\nHurry to the end of the level\nor it's game over!");
        }

    }

    public void TimerUIUpdate(float timeToNextWeather, bool finalWeatherReached)
    {
        
        string minutes = ((int)timeToNextWeather / 60).ToString("D2");
        string seconds = ((int)timeToNextWeather % 60).ToString("D2");
        nextWeatherTimerUI.SetText($"{minutes}:{seconds}");

        if (finalWeatherReached)
        {
            nextWeatherTimerUI.color = Color.red;
        }
    }
}
