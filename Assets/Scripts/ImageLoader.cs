using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ImageLoader
{
    public static void LoadImageTo(string imageUrl, Image toImage, MatchingManager manager)
	{
        
        string cacheName = imageUrl.Substring(imageUrl.LastIndexOf("/"));
        Debug.Log(cacheName);
        string localFilePath = Application.persistentDataPath + cacheName;
        if (File.Exists(localFilePath))
        {
            Debug.Log("Loading cached image");
            byte[] fileData = File.ReadAllBytes(localFilePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            AssignTextureToImage(tex, toImage);
            manager.ImageLoaded(toImage);
        }
        else
        {
            Debug.Log("Downloading image");
            manager.StartCoroutine(DownloadImage(imageUrl, cacheName, toImage, manager));
		}
    }

    static IEnumerator DownloadImage(string imageUrl, string cacheName, Image toImage, MatchingManager manager)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
		yield return request.SendWebRequest();
		if (request.isNetworkError || request.isHttpError)
			Debug.Log(request.error);
		else
		{
			if (request.downloadHandler.data != null)
			{
                AssignTextureToImage(((DownloadHandlerTexture)request.downloadHandler).texture, toImage);
                CacheData(request.downloadHandler.data, cacheName);
                manager.ImageLoaded(toImage);
			}
		}
    }

    private static void AssignTextureToImage(Texture2D tex, Image toImage)
	{
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        toImage.overrideSprite = sprite;
    }
    
    private static void CacheData(byte[] data, string cacheName)
	{
        System.IO.File.WriteAllBytes(Application.persistentDataPath + cacheName, data);
        Debug.Log("Writing Success");
    }
}
