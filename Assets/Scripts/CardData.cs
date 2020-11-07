using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardsData
{
    public static CardsData instance;
    public CardData[] cardsArray;

    public static void LoadFromJson(String text)
	{
        instance = JsonUtility.FromJson<CardsData>(text);
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
}