using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance {get; private set;}

    public int HighScore { get; private set; } = 0;
    public int CurrentScore { get; private set; } = 0;


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
        // Load High Score from PlayerPrefs
        HighScore = PlayerPrefs.GetInt("HighScore", HighScore);
    }

// debugging shortcuts
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameStart();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            GameQuit();
        }
    }
#endif

#region Score Logic
    void SetHighScore(int score)
    {
        if (score > HighScore)
        {
            HighScore = score;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    void IncrementScore()
    {
        CurrentScore++;
    }
#endregion

#region Game State Logic
    public void GameStart()
    {
        Debug.Log("Game Started!");
        CurrentScore = 0;
        Cursor.visible = false;
        SceneManager.LoadScene("Game");
    }

    public void GameLevelComplete()
    {
        Debug.Log("Level Complete!");
        IncrementScore();
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        SetHighScore(CurrentScore);
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public void GameQuit()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
#endregion
}
