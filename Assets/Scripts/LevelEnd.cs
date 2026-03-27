using UnityEngine;

// Simple object for progressing to the next level when touched by the player
public class LevelEnd : MonoBehaviour, IPlayerInteractable
{
    public async void Interact()
    {
        await GameManager.instance.GameLevelComplete();
    }
}
