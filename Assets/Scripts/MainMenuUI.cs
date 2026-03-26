using UnityEngine;
using TMPro;

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
