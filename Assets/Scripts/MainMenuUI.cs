using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreBoard;
    void Start()
    {
        scoreBoard.SetText($"Highest Score Reached: {GameManager.instance.HighScore}");
    }

    public void StartGame()
    {
        GameManager.instance.GameStart();
    }

    public void ExitGame()
    {
        GameManager.instance.GameQuit();
    }
}
