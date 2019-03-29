using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class objectPrefab : MonoBehaviour
{
    string JsonDataString;
    public string OriginalJsonSite;

    public GameObject cluny;
    public GameObject cagedBird;
    public GameObject growUp;
    public GameObject mdhm;
    public GameObject hyestd;
    public GameObject clayCellFracture;

    GameObject Clone;

    // Start is called before the first frame update
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
          //  Debug.Log(kpv.Value["_id"]);
            Debug.Log(kpv.Value["objectID"]);
            //Debug.Log(kpv.Value["rotation"]);
            Debug.Log(kpv.Value["latitude"]);
            Debug.Log(kpv.Value["longitude"]);


            // 1 = cluny
            if (kpv.Value["objectID"].ToString() == "1") {
                Clone = Instantiate(cluny, transform.position, Quaternion.identity) as GameObject;
            } else if (kpv.Value["objectID"].ToString() == "2") {
                Clone = Instantiate(cagedBird, transform.position, Quaternion.identity) as GameObject;
            } else if (kpv.Value["objectID"].ToString() == "3") {
                Clone = Instantiate(growUp, transform.position, Quaternion.identity) as GameObject;
            } else if (kpv.Value["objectID"].ToString() == "4") {
                Clone = Instantiate(mdhm, transform.position, Quaternion.identity) as GameObject;
            } else if (kpv.Value["objectID"].ToString() == "5") {
                Clone = Instantiate(hyestd, transform.position, Quaternion.identity) as GameObject;
            } else if (kpv.Value["objectID"].ToString() == "6") {
                Clone = Instantiate(clayCellFracture, transform.position, Quaternion.identity) as GameObject;
            }


            if (Clone != null) {
                var scriptReference = Clone.GetComponent<ObjectPlacement>();
                if (scriptReference != null) {
                    scriptReference.SetLonLat(kpv.Value["longitude"], kpv.Value["latitude"]);
                }
            }
        
           

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}