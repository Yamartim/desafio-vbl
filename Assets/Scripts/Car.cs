using UnityEngine;

public class Car : MonoBehaviour, IPlayerInteractable
{
    public void Interact()
    {
        GameManager.instance.GameOver();
    }

}
