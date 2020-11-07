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

    private MatchRecords matchRecords;

    void Start ()
	{
        matchRecords = new MatchRecords();
        Utils.ShuffleArray<CardData>(CardsData.instance.cardsArray);
        LoadPair();
    }

    

    public void LoadPair()
	{
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[LeftIndex].front_image, leftButton.image, this);
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[RightIndex].front_image, rightButton.image, this);
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
        if (currentIndex >= RightIndex)
		{
            CallSubmitWins(null);
            currentIndex = 0;
            Utils.ShuffleArray<CardData>(CardsData.instance.cardsArray);
        }
	}

    private void RecordWin(int winnerI, int loserI)
	{
        matchRecords.RecordMatch(CardsData.instance.cardsArray[winnerI].id, CardsData.instance.cardsArray[loserI].id);
    }

    private void CallSubmitWins(Action actionOnDone)
	{
        if (matchRecords.records.Count <=0)
		{
            return; //Nothing to submit
		}
        string jsonString = JsonUtility.ToJson(matchRecords);
        Debug.Log(jsonString);
        StartCoroutine(SubmitWins("http://localhost/keyforgedb/insert_wins.php", jsonString, actionOnDone));
        matchRecords = new MatchRecords();
	}
    private IEnumerator SubmitWins(string url, string jsonString, Action actionOnDone)
    {
       /* var request = new UnityWebRequest(url);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();*/

        WWWForm form = new WWWForm();
        form.AddField("wins_data", jsonString);
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log(request.downloadHandler.text);
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
