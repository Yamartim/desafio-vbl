using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    public static GameManager instance {get; private set;}
    Image screenCover;

    public int HighScore { get; private set; } = 0;
    public int CurrentScore { get; private set; } = 0;
    float fadeDuration = 0.5f;

    bool isGameOver = false;


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
        string ApiUrl = PlayerPrefs.GetString("ApiUrl", ApiRequester.apiUrl);
        ApiRequester.SetApiUrl(ApiUrl);
        // Load High Score from PlayerPrefs
        HighScore = PlayerPrefs.GetInt("HighScore", HighScore);
    }

    async void Start()
    {
        screenCover = GetComponentInChildren<Image>();
        screenCover.color = Color.black;
        await FadeOutScreenCover();

    }

    // debugging shortcuts
#if UNITY_EDITOR
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            await GameStart();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            await GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            await GameQuit();
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            await GameLevelComplete();
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
    public async Task GameStart()
    {
        isGameOver = false;
        await FadeInScreenCover();
        Debug.Log("Game Started!");
        CurrentScore = 0;
        Cursor.visible = false;
        SceneManager.LoadScene("Game");
        await FadeOutScreenCover();
    }

    public async Task GameLevelComplete()
    {
        await FadeInScreenCover();
        Debug.Log("Level Complete!");
        IncrementScore();
        SceneManager.LoadScene("Game");
        await FadeOutScreenCover();
    }

    public async Task GameOver()
    {
        if (isGameOver)
        {
            return;
        }
        isGameOver = true;
        await FadeInScreenCover();
        Debug.Log("Game Over!");
        SetHighScore(CurrentScore);
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
        await FadeOutScreenCover();
    }

    public async Task GameQuit()
    {
        await FadeInScreenCover();
        Debug.Log("Game Quit!");
        Application.Quit();
    }
#endregion

#region UI Logic
    async Task FadeInScreenCover()
    {
        screenCover.gameObject.SetActive(true);
        await screenCover.DOFade(1, fadeDuration)
            .SetEase(Ease.InOutQuad)
            .AsyncWaitForCompletion();
    }

    async Task FadeOutScreenCover()
    {
        await screenCover.DOFade(0, fadeDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => screenCover.gameObject.SetActive(false))
            .AsyncWaitForCompletion();
    }
#endregion
}
