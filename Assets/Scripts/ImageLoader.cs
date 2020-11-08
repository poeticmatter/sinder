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
    public static void LoadImageTo(string imageUrl, Image toImage, MatchingManager manager)
	{
        string cacheName = GetCachedFileName(imageUrl);
        string localFilePath = CachePath + cacheName;
        if (File.Exists(localFilePath))
        {
            byte[] fileData = File.ReadAllBytes(localFilePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            AssignTextureToImage(tex, toImage);
            manager.ImageLoaded(toImage);
        }
        else
        {
            manager.StartCoroutine(DownloadImage(imageUrl, cacheName, toImage, manager));
		}
    }

    private static string GetCachedFileName(string imageUrl)
	{
        return imageUrl.Substring(imageUrl.LastIndexOf("/"));
    }

    public static bool IsCached(string imageUrl)
	{
        Debug.Log(CachePath);
        return File.Exists(CachePath + GetCachedFileName(imageUrl));
    }

    private static IEnumerator DownloadImage(string imageUrl, string cacheName, Image toImage, MatchingManager manager)
    {
        while (imageLoading)
        {
            yield return new WaitForSeconds(0.1f);
        }
        imageLoading = true;
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
                if (manager!= null)
				{
                    manager.ImageLoaded(toImage);
				}
			}
		}
        imageLoading = false;
    }

    public static IEnumerator DownloadImage(string imageUrl)
    {
        while (imageLoading)
		{
            yield return new WaitForSeconds(1f);
        }
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            if (request.downloadHandler.data != null)
            {
                CacheData(request.downloadHandler.data, GetCachedFileName(imageUrl));
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
    }
}
