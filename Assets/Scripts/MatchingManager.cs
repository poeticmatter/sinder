using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class MatchingManager : MonoBehaviour
{
    public Button left;
    public Button right;
    private int currentI;
    bool leftLoaded = false;
    bool rightLoaded = false;

    void Start ()
	{
        ShuffleArray<CardData>(CardsData.instance.cardsArray);
        LoadPair();
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public void LoadPair()
	{
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[currentI].front_image, left.image, this);
        ImageLoader.LoadImageTo(CardsData.instance.cardsArray[currentI + CardsData.instance.cardsArray.Length / 2].front_image, right.image, this);
        Debug.Log(currentI + " " + (currentI + CardsData.instance.cardsArray.Length / 2));
    }

    public void ImageLoaded(Image loaded)
	{
        if (left.image == loaded)
		{
            leftLoaded = true;
		} else if (right.image == loaded)
		{
            rightLoaded = true;
		}
        if (leftLoaded && rightLoaded)
		{
            left.interactable = true;
            right.interactable = true;
            leftLoaded = false;
            rightLoaded = false;
		}
	}

    public void LeftClicked()
	{
        DoClick(currentI, CardsData.instance.cardsArray.Length / 2);
    }

    public void RightClicked()
	{
        DoClick(CardsData.instance.cardsArray.Length / 2, currentI);
    }

    private void DoClick(int winI, int lossI)
	{
        left.interactable = false;
        right.interactable = false;
        RecordWin(winI, lossI);
        Increment();
        LoadPair();
    }

    private void Increment()
	{
        currentI++;
        if (currentI >= CardsData.instance.cardsArray.Length / 2)
		{
            currentI = 0;
            ShuffleArray<CardData>(CardsData.instance.cardsArray);
        }
	}

    private void RecordWin(int winnerI, int loserI)
	{
        //Record
	}

    private void CallSubmitWins()
	{

	}
    private IEnumerator SubmitWins(string url, string jsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
        }

    }
}
