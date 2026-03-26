using UnityEngine;

public class LevelEnd : MonoBehaviour, IPlayerInteractable
{
    public void Interact()
    {
        GameManager.instance.GameLevelComplete();
    }
}
