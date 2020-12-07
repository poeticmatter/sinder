using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class RankingCounter
{
    public static IEnumerator GetRankingCount(int expansionNumber, Text text, UnityEvent<Text,string> onFinish)
    {
        WWWForm form = new WWWForm();
        form.AddField("expansion", expansionNumber);
        UnityWebRequest request = UnityWebRequest.Post(URLs.RANKING_COUNT, form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else if (request.downloadHandler.text.StartsWith("count="))
        {
            Debug.Log("Transactions Recieved");
            onFinish?.Invoke(text, request.downloadHandler.text.Substring(6));
        } else
		{
            Debug.Log(request.downloadHandler.text);
		}
    }
}
