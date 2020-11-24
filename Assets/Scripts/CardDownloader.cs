using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDownloader : MonoBehaviour
{
    public Dropdown expansionField;
    //341 = CotA
    //425 = AoA
    //452 = WC
    //479 = MM
    private static int[] expansionNumbers = new int[] { 341, 435, 452, 479 };

    public void RankCardsClicked()
    {
        StartCoroutine(GetCards(GoToMatching));
    }

    public void DisplayRankingClicked()
    {
        StartCoroutine(GetCards(GoToDisplay));
    }

    public void QuitClicked()
	{
        Application.Quit();
	}

    IEnumerator GetCards(Action onFinish)
    {
        WWWForm form = new WWWForm();
        form.AddField("expansion", expansionNumbers[expansionField.value]);
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
            CardsData.expansion = expansionNumbers[expansionField.value];
            onFinish();
		}
    }

    private void GoToMatching()
	{
        SceneManager.LoadScene(1);
    }

    private void GoToDisplay()
    {
        Application.OpenURL(URLs.CARD_RANKING +"?expansion="+ expansionNumbers[expansionField.value]);
    }

}
