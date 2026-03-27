using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float baseSpeed = 7f;
    [SerializeField] float smoothTurnTime = 0.1f;
    CharacterController characterController;
    Camera mainCamera;
    float turnVelocity;
    float weatherSpeedModifier = 1f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WeatherController.OnWeatherChange += OnWeatherChange;
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        OnWeatherChange(ApiRequester.lastWeatherData.current_status);
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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(move.normalized * baseSpeed * weatherSpeedModifier * Time.deltaTime);
        }
    }

    void OnWeatherChange(Status weatherStatus)
    {
        WeatherCondition condition = weatherStatus.GetWeatherCondition();
        switch (condition)
        {
            case WeatherCondition.Sunny:
                weatherSpeedModifier = 1f;
                break;
            case WeatherCondition.Clouded:
                weatherSpeedModifier = 0.8f;
                break;
            case WeatherCondition.Foggy:
                weatherSpeedModifier = 0.8f;
                break;
            case WeatherCondition.LightRain:
                weatherSpeedModifier = 0.6f;
                break;
            case WeatherCondition.HeavyRain:
                weatherSpeedModifier = 0.4f;
                break;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent(out IPlayerInteractable interactable))
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
