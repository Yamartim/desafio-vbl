using TMPro;
using UnityEngine;

// UI class used for testing the API before the game starts
public class ApiTesterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField urlInputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        urlInputField.text = ApiRequester.apiUrl;
    }

    public async void TestApi()
    {
        ApiRequester.SetApiUrl(urlInputField.text);

        RequestResult result = await ApiRequester.GetRequest();
        resultText.SetText($"Last Request Result: \n{result.HttpResult}");

        if (result.Success)
        {
            resultText.color = Color.green;
            resultText.text += $"\n\n{ApiRequester.lastWeatherData}";
        }
        else
        {
            resultText.color = Color.red;
        }
    }
}
