using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDownloader : MonoBehaviour
{
    public Dropdown expansionField;
    public Dropdown houseField;

    public void CallGetCards()
    {
        StartCoroutine(GetCards());
    }

    IEnumerator GetCards()
    {
        WWWForm form = new WWWForm();
        form.AddField("expansion", expansionField.options[expansionField.value].text);
        form.AddField("house", houseField.options[houseField.value].text);
        UnityWebRequest request = UnityWebRequest.Post("http://localhost/keyforgedb/get_cards.php", form);
        yield return request.SendWebRequest();
        CardsData.LoadFromJson(request.downloadHandler.text);
        SceneManager.LoadScene(1);
    }
}
