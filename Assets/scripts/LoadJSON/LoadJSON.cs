using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class LoadJSON : MonoBehaviour
{
    string JsonDataString;
    public string OriginalJsonSite;
    public Text UserNames;

    IEnumerator Start()
    {
        WWW readingsite = new WWW(OriginalJsonSite);
        yield return readingsite;

        if (string.IsNullOrEmpty(readingsite.error))
        {
            JsonDataString = readingsite.text;
        }

        JSONNode jsonNode = SimpleJSON.JSON.Parse(JsonDataString);
        Debug.Log("TRY TO LOAD LOCATION!");

        foreach (var kpv in jsonNode)
        {
            UserNames.text += kpv.Key.ToString().ToUpper();
            // Debug.Log(jsonNode["country"]);
            // Debug.Log(key + " = " + key["latitude"].ToString());

        }

       // CountryName.text = jsonNode["country"].ToString().ToUpper();
       //Debug.Log(jsonNode["country"]);
    }
}