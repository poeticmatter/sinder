using UnityEngine;
using System.Runtime.InteropServices;

public class LinkOpener : MonoBehaviour
{

    public static void OpenLinkJSPlugin(string url)
    {
#if !UNITY_EDITOR
		openWindow(url);
#endif
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);
}
