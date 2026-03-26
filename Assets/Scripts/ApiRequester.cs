using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    public static async Task<UnityWebRequest.Result> GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(ApiUrl))
        {
            // Request and wait for the desired page.
            await webRequest.SendWebRequest();

            string[] pages = ApiUrl.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    LastWeatherData = WeatherData.FromJson(webRequest.downloadHandler.text);
                    break;
            }
            return webRequest.result;
        }
    }
}