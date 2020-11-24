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
    bool leftLoaded = false;
    bool rightLoaded = false;

    private TransactionRecords transactionRecords;

    void Start ()
	{
        transactionRecords = new TransactionRecords();
        Utils.ShuffleArray<CardData>(CardsData.instance.cardsArray);
        LoadPair();
        
    }

    private void PreloadNextPair()
    {
        if (currentIndex + 1 < CardsData.instance.cardsArray.Length / 2)
		{
            if (!ImageLoader.IsCached(CardsData.instance.cardsArray[LeftIndex + 1].front_image))
		    {
                StartCoroutine(ImageLoader.DownloadImage(CardsData.instance.cardsArray[LeftIndex + 1].front_image));
            }
            if (!ImageLoader.IsCached(CardsData.instance.cardsArray[RightIndex + 1].front_image))
            {
                StartCoroutine(ImageLoader.DownloadImage(CardsData.instance.cardsArray[RightIndex + 1].front_image));
            }
        }
	
    }

    public void LoadPair()
	{
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[LeftIndex].front_image, leftButton.image, this);
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[RightIndex].front_image, rightButton.image, this);
        PreloadNextPair();
    }

    public void ImageLoaded(Image loaded)
	{
        if (leftButton.image == loaded)
		{
            leftLoaded = true;
		} else if (rightButton.image == loaded)
		{
            rightLoaded = true;
		}
        if (leftLoaded && rightLoaded)
		{
            leftButton.interactable = true;
            rightButton.interactable = true;
            leftLoaded = false;
            rightLoaded = false;
		}
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
        leftButton.interactable = false;
        rightButton.interactable = false;
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
            if (actionOnDone!=null)
			{
                actionOnDone();
			}
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
