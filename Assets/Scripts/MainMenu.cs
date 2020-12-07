using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public class TextUpdateEvent : UnityEvent<Text, string>
    {
    }

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
	public int expnasionIndexUsed { get; private set; }

    //341 = CotA
    //425 = AoA
    //452 = WC
    //479 = MM
    private static int[] expansionNumbers = new int[] { 341, 435, 452, 479 };

    public void RankCardsClicked(int expnasionIndex)
    {
        expnasionIndexUsed = expnasionIndex;
        StartCoroutine(CardDownloader.GetCards(expansionNumbers[expnasionIndex], GoToMatching));
    }

    public void DisplayRankingClicked(int expnasionIndex)
    {
        expnasionIndexUsed = expnasionIndex;
        StartCoroutine(CardDownloader.GetCards(expansionNumbers[expnasionIndex], GoToDisplay));
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    private void GoToMatching()
    {
        SceneManager.LoadScene(1);
    }

    private void GoToDisplay()
    {
        LinkOpener.OpenLinkJSPlugin(URLs.CARD_RANKING + "?expansion=" + expansionNumbers[expnasionIndexUsed]);
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
