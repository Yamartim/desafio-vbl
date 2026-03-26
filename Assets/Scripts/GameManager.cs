using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance {get; private set;}

    public WeatherData CurrentWeatherData { get; private set; }

    public int HighScore { get; private set; } = 0;


    void Awake()
    {
        // configure singleton pattern
        if (instance is not null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // Load API URL from PlayerPrefs or use default if not set
        string ApiUrl = PlayerPrefs.GetString("ApiUrl", ApiRequester.ApiUrl);
        ApiRequester.SetApiUrl(ApiUrl);
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChangeToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetHighScore(int score)
    {
        HighScore = score > HighScore ? score : HighScore;
    }

}
