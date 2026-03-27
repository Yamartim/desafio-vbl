using UnityEngine;
using TMPro;

// Basic UI class for main menu functions
// it's useful because we can't reference gameManager directly when configuring UI buttons since the reference is lost when coming back from another scene
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreBoard;
    void Start()
    {
        scoreBoard.SetText($"Highest Score Reached: {GameManager.instance.HighScore}");
    }

    public async void StartGame()
    {
        await GameManager.instance.GameStart();
    }

    public async void ExitGame()
    {
        await GameManager.instance.GameQuit();
    }
}
