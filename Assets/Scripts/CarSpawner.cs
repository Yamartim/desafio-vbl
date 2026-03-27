using System.Collections.Generic;
using System.Linq;
using UnityEditor.Embree;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] int carLimit = 10;
    [SerializeField] float spawnFrequancy = 0.1f;
    [SerializeField] GameObject carPrefab;

    Queue<GameObject> inactiveCars;

    int carCount = 0;
    float weatherSpawnModifier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inactiveCars = new Queue<GameObject>();
        WeatherController.OnWeatherChange += OnWeatherChange;
        OnWeatherChange(ApiRequester.lastWeatherData.current_status);
    }


    void OnWeatherChange(Status weatherStatus)
    {
        weatherSpawnModifier = (float)weatherStatus.vehicleDensity;
        CancelInvoke(nameof(SpawnCar));
        InvokeRepeating(nameof(SpawnCar), Random.value, 1f/spawnFrequancy * weatherSpawnModifier);
    }

    void SpawnCar()
    {
        if (carCount < carLimit)
        {
            GameObject newCar = Instantiate(carPrefab, this.transform);
            newCar.transform.forward = this.transform.forward;
            carCount++;
        } 
        else
        {
            if (inactiveCars.Count == 0)
            {
                return;
            }
            GameObject reusedCar = inactiveCars.Dequeue();

            reusedCar.transform.position = this.transform.position;
            reusedCar.transform.forward = this.transform.forward;
            Car car = reusedCar.GetComponent<Car>();
            reusedCar.SetActive(true);
            car.SetupCar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            inactiveCars.Enqueue(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
