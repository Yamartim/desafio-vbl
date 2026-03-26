using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiRequester
{
    public static string ApiUrl { get; private set; } = "http://localhost:3002/v1/traffic/status";
    public static WeatherData LastWeatherData { get; private set; }

    public static void SetApiUrl(string url)
    {
        ApiUrl = url;
        PlayerPrefs.SetString("ApiUrl", url);
    }

    public static async Task<RequestResult> GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(ApiUrl))
        {
            // Request and wait for the desired page.
            await webRequest.SendWebRequest();

            string[] pages = ApiUrl.Split('/');
            int page = pages.Length - 1;

            RequestResult result;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    result = new RequestResult(webRequest.result, null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    result = new RequestResult(webRequest.result, null);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    LastWeatherData = WeatherData.FromJson(webRequest.downloadHandler.text);
                    result = new RequestResult(webRequest.result, LastWeatherData);
                    break;
                default:
                    Debug.LogError(pages[page] + ": Unknown Error");
                    result = new RequestResult(webRequest.result, null);
                    break;
            }

            return result;
        }
    }
}

public class RequestResult
{
    public bool Success { get; private set; }
    public UnityWebRequest.Result HttpResult { get; private set; }
    public WeatherData WeatherData { get; private set; }

    public RequestResult(UnityWebRequest.Result result, WeatherData weatherData)
    {
        Success = result == UnityWebRequest.Result.Success;
        HttpResult = result;
        WeatherData = weatherData;
    }
}