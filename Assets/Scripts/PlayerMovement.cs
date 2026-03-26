using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float WeatherSpeedModifier = 1f;

    float baseSpeed = 5f;

    CharacterController characterController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WeatherController.OnWeatherChange += OnWeatherChange;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }

    void MovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 move = direction * baseSpeed * WeatherSpeedModifier * Time.deltaTime;
            characterController.Move(move);
        }
    }

    void OnWeatherChange(WeatherCondition condition)
    {
        switch (condition)
        {
            case WeatherCondition.Sunny:
                WeatherSpeedModifier = 1f;
                break;
            case WeatherCondition.Clouded:
                WeatherSpeedModifier = 0.8f;
                break;
            case WeatherCondition.Foggy:
                WeatherSpeedModifier = 0.8f;
                break;
            case WeatherCondition.LightRain:
                WeatherSpeedModifier = 0.6f;
                break;
            case WeatherCondition.HeavyRain:
                WeatherSpeedModifier = 0.4f;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayerInteractable interactable))
        {
            interactable.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayerInteractable interactable))
        {
            interactable.Interact();
        }
    }
}
