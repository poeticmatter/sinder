using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        Application.OpenURL(URLs.CARD_RANKING + "?expansion=" + expansionNumbers[expnasionIndexUsed]);
    }
}
