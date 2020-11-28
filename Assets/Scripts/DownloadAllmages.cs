using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class DownloadAllmages : MonoBehaviour
{
    //341 = CotA
    //425 = AoA
    //452 = WC
    //453 = Anomalies
    //479 = MM
    private static int[] expansionNumbers = new int[] { 341, 435, 452, 453, 479 };
    void Start()
    {
        StartCoroutine(GetALLCards());
    }


    IEnumerator GetALLCards()
    {
		for (int i = 0; i < expansionNumbers.Length; i++)
		{
            WWWForm form = new WWWForm();
            form.AddField("expansion", expansionNumbers[i]);
            UnityWebRequest request = UnityWebRequest.Post(URLs.GET_CARDS, form);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Cards Recieved");
                Debug.Log(request.downloadHandler.text);
                CardsData.LoadFromJson(request.downloadHandler.text);
                CardsData.expansion = expansionNumbers[i];

                for (int h = 0; h < CardsData.instance.cardsArray.Length; h++)
			    {
                    CardData card = CardsData.instance.cardsArray[h];
                    request = UnityWebRequestTexture.GetTexture(card.front_image);
                    yield return request.SendWebRequest();
                    if (request.isNetworkError || request.isHttpError)
                        Debug.Log(request.error);
                    else
                    {
                        if (request.downloadHandler.data != null)
                        {
                            CacheData(request.downloadHandler.data, GetCachedFileName(card));
                        }
                    }
                }
            
            }
		}
    }

    public static IEnumerator DownloadImage(CardData card)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(card.front_image);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            if (request.downloadHandler.data != null)
            {
                CacheData(request.downloadHandler.data, GetCachedFileName(card));
            }
        }
    }

    private static void CacheData(byte[] data, string cacheName)
    {
        Debug.Log( cacheName);
        File.WriteAllBytes(cacheName, data);
    }

    private static string GetCachedFileName(CardData card)
    {
        string dir = Application.persistentDataPath + "/" + CardsData.expansion + "/" + card.house + "/";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return dir  + card.id +".png";
    }

}
