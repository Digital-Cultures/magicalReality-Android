using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class LoadJSON : MonoBehaviour
{
    string JsonDataString;
    public string OriginalJsonSite;
    public Text CountryName;

    IEnumerator Start()
    {
        WWW readingsite = new WWW(OriginalJsonSite);
        yield return readingsite;

        if (string.IsNullOrEmpty(readingsite.error))
        {
            JsonDataString = readingsite.text;
        }

        JSONNode jsonNode = SimpleJSON.JSON.Parse(JsonDataString);

        foreach (var kpv in jsonNode)
        {
            // CountryName.text = jsonNode["country"].ToString().ToUpper();
            // Debug.Log(jsonNode["country"]);
            // Debug.Log(key + " = " + key["latitude"].ToString());
            Debug.Log(kpv.Value["_id"]);
            Debug.Log(kpv.Value["objectID"]);
            Debug.Log(kpv.Value["rotation"]);
            Debug.Log(kpv.Value["latitude"]);
            Debug.Log(kpv.Value["longitude"]);

        }

       // CountryName.text = jsonNode["country"].ToString().ToUpper();
       //Debug.Log(jsonNode["country"]);
    }
}