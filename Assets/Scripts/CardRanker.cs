using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardRanker : MonoBehaviour
{
    private static readonly int K = 10;

    public Text textArea;
    private TransactionRecords transactionsRecords;
    void Start()
    {
        StartCoroutine(GetTransactions(ComputeRanking));
    }
    IEnumerator GetTransactions(Action onFinish)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post("http://localhost/keyforgedb/get_transaction_records.php", form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Transactions Recieved");
            transactionsRecords = JsonUtility.FromJson<TransactionRecords>(request.downloadHandler.text);
			onFinish?.Invoke();
		}
    }

    private void ComputeRanking()
	{
		for (int i = 0; i < transactionsRecords.transactions.Count; i++)
		{
            string winId = transactionsRecords.transactions[i].card_win_id;
            string loseID = transactionsRecords.transactions[i].card_lose_id;
            if (CardsData.instance.idToCardData.ContainsKey(winId) && CardsData.instance.idToCardData.ContainsKey(loseID))
			{
                int scoreWin = CardsData.instance.idToCardData[winId].score;
                int scoreLose = CardsData.instance.idToCardData[loseID].score;
                CardsData.instance.idToCardData[winId].score = EloCalculation(scoreWin, scoreLose, 1);
                CardsData.instance.idToCardData[loseID].score = EloCalculation(scoreLose, scoreWin, 0);
			}
        }
        Array.Sort<CardData>(CardsData.instance.cardsArray, new Comparison<CardData>((i1, i2) => i2.CompareTo(i1)));
        for (int i = 0; i < CardsData.instance.cardsArray.Length; i++)
		{
            textArea.text += CardsData.instance.cardsArray[i].card_title + " " + CardsData.instance.cardsArray[i].score + "\n";
        }

	}

    private int EloCalculation(int scoreA, int scoreB, int result)
	{
        double expectedScore = 1/ (1 + Math.Pow(10, (double)(scoreB - scoreA) / 400));
        return (int)(scoreA + K * (result - expectedScore));
    }

    public void ClickBack()
    {
        SceneManager.LoadScene(0);
    }
}
