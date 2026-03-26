using UnityEngine;

public class Car : MonoBehaviour, IPlayerInteractable
{
    Rigidbody rigidBody;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] float speed = 10f;
    double weatherSpeedModifier = 1.0;

    void Start()
    {
        WeatherController.OnWeatherChange += OnWeatherChange;

        rigidBody = GetComponent<Rigidbody>();
        
        SetupCar();
    }

    void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + transform.forward * speed * (float)weatherSpeedModifier);
    }

    public async void Interact()
    {
        await GameManager.instance.GameOver();
    }

    void OnWeatherChange(Status weatherStatus)
    {
        SetWeatherSpeedModifier(weatherStatus.averageSpeed);
    }

    void OnEnable()
    {
        SetupCar();
    }
    public void SetWeatherSpeedModifier(double modifier)
    {
        weatherSpeedModifier = modifier;
    }

    public void SetupCar()
    {
        transform.forward = Vector3.forward; 

        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material.color = randomColor;
        }
    }
}
