using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardDownloader
{
    public static IEnumerator GetCards(int expansionNumber, Action onFinish)
    {
        WWWForm form = new WWWForm();
        form.AddField("expansion", expansionNumber);
        UnityWebRequest request = UnityWebRequest.Post(URLs.GET_CARDS, form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
		{
            Debug.Log("Cards Recieved");
            CardsData.LoadFromJson(request.downloadHandler.text);
            CardsData.expansion = expansionNumber;
            onFinish();
		}
    }


}
