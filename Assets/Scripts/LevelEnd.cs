using System.Threading.Tasks;
using UnityEngine;

public class LevelEnd : MonoBehaviour, IPlayerInteractable
{
    public async void Interact()
    {
        await GameManager.instance.GameLevelComplete();
    }
}
