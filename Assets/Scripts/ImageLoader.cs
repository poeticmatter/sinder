using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ImageLoader
{
    private static string CachePath { get { return Application.persistentDataPath; } }
    private static bool imageLoading = false;

    public static void LoadImageTo(CardData card, Image toImage)
    {
        Texture2D texture = Resources.Load<Texture2D>("Cards/" + CardsData.expansion + "/" + card.house + "/" + card.id);
        if (texture == null && CardsData.expansion == 452 /*WC*/)
        {
            texture = Resources.Load<Texture2D>("Cards/" + 453 /*Anomalies*/ + "/" + card.house + "/" + card.id);
        }

        AssignTextureToImage(texture, toImage);
    }

    private static void AssignTextureToImage(Texture2D tex, Image toImage)
	{
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        toImage.overrideSprite = sprite;
    }
 
}
