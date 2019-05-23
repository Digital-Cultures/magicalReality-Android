using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SimpleJSON;

using ImaginationOverflow.UniversalDeepLinking;
using UnityEngine.Networking;

public class LoadJSON : MonoBehaviour
{
    string JsonDataString;
    //private string OriginalJsonSite = "https://digitalcultures.ncl.ac.uk/projects/magicalreality/";
    private string OriginalJsonSite = "https://digitalcultures.ncl.ac.uk/projects/magicalreality/site/php/get_all_routes.php";

    private JSONNode userJson;
    private IEnumerator coroutine;

    public Text debugText;

    [Tooltip("populate with user routes")]
    public Dropdown dropdown;

    private void Awake()
    {
        dropdown.GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(index =>
        {
            Global.chosenRoute = dropdown.options[dropdown.value].text;
        }));
    }

    void Start()
    {
        DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;

        // WWW readingsite = new WWW(OriginalJsonSite + userid + ".json");
        // yield return readingsite;

        // if (string.IsNullOrEmpty(readingsite.error))
        // {
        //     JsonDataString = readingsite.text;
        // }

        // JSONNode users = SimpleJSON.JSON.Parse(JsonDataString);
        // Debug.Log("TRY TO LOAD LOCATION!");

        // dropdown.ClearOptions();

        // List<Dropdown.OptionData> routes = new List<Dropdown.OptionData>();

        // //
        // foreach (var user in users["users"])
        // {
        //     // UserNames.text += kpv.Key.ToString().ToUpper();

        //     Debug.Log(user.Value["user"].ToString());

        //     foreach (var routesJSON in user.Value["routes"])
        //     {
        //         Debug.Log(routesJSON.Value["routeName"].ToString());
        //         var route = new Dropdown.OptionData(routesJSON.Value["routeName"].ToString().ToUpper());
        //         routes.Add(route);
        //         // Debug.Log(jsonNode["country"]);
        //         // Debug.Log(key + " = " + key["latitude"].ToString());
        //     }

        // }
        // dropdown.AddOptions(routes);
        //// CountryName.text = jsonNode["country"].ToString().ToUpper();
        //Debug.Log(jsonNode["country"]);
    }

    private void Instance_LinkActivated(LinkActivation linkActivation)
    {
        //
        //  my activation code
        //
        var uri = linkActivation.Uri;
        var querystring = linkActivation.RawQueryString;
        Global.userid = linkActivation.QueryString["user"];
        Global.walkid = linkActivation.QueryString["walkid"];

        //TODO: if walkid==null show dropdown

        //Debug.Log(OriginalJsonSite);


        Debug.Log("TRY TO LOAD FROM LINK IN "+ SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "main")
        {
            SceneManager.LoadScene("mainMenu");
        }

        coroutine = GetRequest(OriginalJsonSite);
        //coroutine = GetRequest(OriginalJsonSite + userid + ".json");
        StartCoroutine(coroutine);
    }

    IEnumerator GetRequest(string uri)
    {
        Debug.Log("Start request :" + uri);

        UnityWebRequest www = UnityWebRequest.Get(uri);

        // Request and wait for the desired page.
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(": Error: " + www.error);
        }
        else
        {
            UpdateDropdown(www.downloadHandler.text);
            Debug.Log("Received: " + www.downloadHandler.text);
        }

    }

    public void UpdateDropdown(string JsonDataString)
    {
        userJson = SimpleJSON.JSON.Parse("{\n\"users\": " + JsonDataString + "}");

        dropdown.ClearOptions();

        List<Dropdown.OptionData> routes = new List<Dropdown.OptionData>();

        //
        foreach (var user in userJson["users"])
        {
            // UserNames.text += kpv.Key.ToString().ToUpper();

            Debug.Log(Global.userid + "  =  " + user.Value["user"].ToString().Substring(1, user.Value["user"].ToString().Length - 2));

            if (Global.userid == user.Value["user"].ToString().Substring(1, user.Value["user"].ToString().Length - 2))
            {
                foreach (var routesJSON in user.Value["routes"])
                {
                    Debug.Log(routesJSON.Value["routeName"].ToString());
                    var route = new Dropdown.OptionData(routesJSON.Value["routeName"].ToString().ToUpper());
                    routes.Add(route);

                    if (Global.chosenRoute == "")
                    {
                        Global.chosenRoute = routesJSON.Value["routeName"].ToString().ToUpper();
                    }
                }
            }
        }

        dropdown.AddOptions(routes);
    }
}