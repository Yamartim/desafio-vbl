using UnityEngine;

public class Car : MonoBehaviour, IPlayerInteractable
{
    public async void Interact()
    {
        await GameManager.instance.GameOver();
    }

}
