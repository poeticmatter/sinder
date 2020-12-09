using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Serializable]
    public class TextUpdateEvent : UnityEvent<Text, string> { }

    public Text[] showRankingButtonText;
	void Start()
	{
        FireGetRankingCount(0);
        FireGetRankingCount(1);
        FireGetRankingCount(2);
        FireGetRankingCount(3);
    }

    private void FireGetRankingCount(int expansionIndex)
	{
        UnityEvent<Text, string> updateEvent = new TextUpdateEvent();
        updateEvent.AddListener(UpdateButtonText);
        StartCoroutine(RankingCounter.GetRankingCount(expansionNumbers[expansionIndex], showRankingButtonText[expansionIndex], updateEvent));
    }

    //341 = CotA
    //425 = AoA
    //452 = WC
    //479 = MM
    private static int[] expansionNumbers = new int[] { 341, 435, 452, 479 };

    public void RankCardsClicked(int expnasionIndex)
    {
        StartCoroutine(CardDownloader.GetCards(expansionNumbers[expnasionIndex], GoToMatching));
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    private void GoToMatching()
    {
        SceneManager.LoadScene(1);
    }

    public void DisplayRankingClicked(int expansionIndex)
    {
        LinkOpener.OpenLinkJSPlugin(URLs.CARD_RANKING + "?expansion=" + expansionNumbers[expansionIndex]);
    }

    private void UpdateButtonText(Text text, string count)
	{
        text.text = text.text.Replace("000", count);
    }

    public void GotoGitHub()
	{
        LinkOpener.OpenLinkJSPlugin("https://github.com/poeticmatter/sinder");
	}
}
