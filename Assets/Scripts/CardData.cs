using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardsData
{
    public static CardsData instance;
    public CardData[] cardsArray;
    [NonSerialized()]
    public Dictionary<string, CardData> idToCardData;
    [NonSerialized()]
    public static int expansion;

    public static void LoadFromJson(String text)
	{
        instance = JsonUtility.FromJson<CardsData>(text);
        instance.InitDictionary();
    }

    private void InitDictionary()
	{
        idToCardData = new Dictionary<string, CardData>();
		for (int i = 0; i < cardsArray.Length; i++)
		{
            idToCardData.Add(cardsArray[i].id, cardsArray[i]);
		}

    }

    public static bool IsLoaded()
	{
        return instance != null;
	}
}
[Serializable]
public class CardData
{
    public string id;
    public string card_title;
    public string front_image;
    public string rarity;
    public string house;
    [NonSerialized()]
    public int score = 1500;

	internal int CompareTo(CardData i1)
	{
        return score.CompareTo(i1.score);
	}
}