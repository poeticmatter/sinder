using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class MatchingManager : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    private int currentIndex = 0;
    private int LeftIndex { get { return currentIndex; } }
    private int RightIndex { get { return currentIndex + CardsData.instance.cardsArray.Length / 2; } }

    private TransactionRecords transactionRecords;

    void Start ()
	{
        transactionRecords = new TransactionRecords();
        Utils.ShuffleArray<CardData>(CardsData.instance.cardsArray);
        LoadPair();
        
    }

    public void LoadPair()
	{
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[LeftIndex], leftButton.image);
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[RightIndex], rightButton.image);
    }


    public void LeftClicked()
	{
        DoClick(LeftIndex, RightIndex);
    }

    public void RightClicked()
	{
        DoClick(RightIndex, LeftIndex);
    }

    private void DoClick(int winI, int lossI)
	{
        RecordWin(winI, lossI);
        Increment();
        LoadPair();
    }

    private void Increment()
	{
        currentIndex++;
        if (currentIndex >= CardsData.instance.cardsArray.Length / 2)
		{
            currentIndex = 0;
            Utils.ShuffleArray<CardData>(CardsData.instance.cardsArray);
        }
        if (currentIndex % 25 ==0)
		{
            CallSubmitWins(null);
        }
	}

    private void RecordWin(int winnerI, int loserI)
	{
        transactionRecords.RecordTransaction(CardsData.instance.cardsArray[winnerI].id, CardsData.instance.cardsArray[loserI].id);
    }

    private void CallSubmitWins(Action actionOnDone)
	{
        if (transactionRecords.transactions.Count <=0)
		{
            actionOnDone?.Invoke();
            return; //Nothing to submit
		}
        string jsonString = JsonUtility.ToJson(transactionRecords);
        StartCoroutine(SubmitTransactions(URLs.INSERT_TRANSACTIONS, jsonString, actionOnDone));
        transactionRecords = new TransactionRecords();
	}
    private IEnumerator SubmitTransactions(string url, string jsonString, Action actionOnDone)
    {
        WWWForm form = new WWWForm();
        form.AddField("transactions_root", jsonString);
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Submitted Transactions");
			actionOnDone?.Invoke();
		}

    }

    public void ClickBack()
	{
        CallSubmitWins(ExecuteBack);
	}

    public void ExecuteBack()
	{
        SceneManager.LoadScene(0);
    }
}
