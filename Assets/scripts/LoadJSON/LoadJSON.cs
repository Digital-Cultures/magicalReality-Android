using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SimpleJSON;

using ImaginationOverflow.UniversalDeepLinking;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadJSON : MonoBehaviour
{
    string JsonDataString;
    //private string OriginalJsonSite = "https://digitalcultures.ncl.ac.uk/projects/magicalreality/";
    private string OriginalJsonSite = "https://magicalreality.ncl.ac.uk/php/get_all_routes.php";

    private JSONNode userJson;
    private IEnumerator coroutine;

    public Text debugText;
    public Text walkName;
    public GameObject startBtn;

    [Tooltip("populate with user routes")]
    public Dropdown dropdown;

    private void Awake()
    {
        Debug.Log("AWAKE");
        dropdown.GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(index =>
        {
            Global.chosenRoute = dropdown.options[dropdown.value].text;
            walkName.text = dropdown.options[dropdown.value].text;
        }));

    }

    void Start()
    {
        Debug.Log("START");

        startBtn.SetActive(false);
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

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "mainMenu" && Global.userid != "")
        {
            coroutine = GetRequest(OriginalJsonSite);
            //coroutine = GetRequest(OriginalJsonSite + userid + ".json");
            StartCoroutine(coroutine);
            //dropdown.value = Global.walkid - 1;
            //Global.chosenRoute = dropdown.options[dropdown.value].text;
        }
        Debug.Log("Level Loaded = "+ scene.name);
        Debug.Log("Global.useridd = " + Global.userid);
        Debug.Log("Global.walkid = " + Global.walkid);

    }

    private void Instance_LinkActivated(LinkActivation linkActivation)
    {
        //
        //  my activation code
        //
        var uri = linkActivation.Uri;
        var querystring = linkActivation.RawQueryString;
        Global.userid = linkActivation.QueryString["user"];
        Global.walkid = int.Parse(linkActivation.QueryString["walkid"]);

        //TODO: if walkid==null show dropdown

        //Debug.Log(OriginalJsonSite);


        Debug.Log("TRY TO LOAD FROM LINK IN "+ SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "main")
        {
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            coroutine = GetRequest(OriginalJsonSite);
            //coroutine = GetRequest(OriginalJsonSite + userid + ".json");
            StartCoroutine(coroutine);
        }
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

                    // this sets the defalt to the last route
                    if (Global.chosenRoute == "")
                    {
                        Global.chosenRoute = routesJSON.Value["routeName"].ToString().ToUpper();
                    }
                }
            }
        }

        dropdown.AddOptions(routes);
        dropdown.value = Global.walkid - 1;
        Global.chosenRoute = dropdown.options[dropdown.value].text;
        var walkNameText = dropdown.options[dropdown.value].text;
        walkName.text = walkNameText.Substring(1, walkNameText.Length - 2) + Environment.NewLine + "by" + Environment.NewLine + Global.userid;

        startBtn.SetActive(true);
    }

}