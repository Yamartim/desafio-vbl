using TMPro;
using UnityEngine;

public class ApiTester : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField urlInputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //urlInputField.text = ApiRequester.ApiUrl;
        urlInputField.text = ApiRequester.apiUrl;
    }

    // Update is called once per frame
    public async void TestApi()
    {
        //ApiRequester.SetApiUrl(urlInputField.text);
        ApiRequester.SetApiUrl(urlInputField.text);

        RequestResult result = await ApiRequester.GetRequest();
        resultText.text = $"Last Request Result: \n{result.HttpResult}";

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
