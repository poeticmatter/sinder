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

    public void RankCardsClicked()
    {
        StartCoroutine(GetCards(GoToMatching));
    }

    public void DisplayRankingClicked()
    {
        StartCoroutine(GetCards(GoToDisplay));
    }

    IEnumerator GetCards(Action onFinish)
    {
        WWWForm form = new WWWForm();
        form.AddField("expansion", expansionField.options[expansionField.value].text);
        UnityWebRequest request = UnityWebRequest.Post("http://localhost/keyforgedb/get_cards.php", form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
		{
            Debug.Log("Cards Recieved");
            CardsData.LoadFromJson(request.downloadHandler.text);
            onFinish();
		}
    }

    private void GoToMatching()
	{
        SceneManager.LoadScene(1);
    }

    private void GoToDisplay()
    {
        SceneManager.LoadScene(2);
    }


}
