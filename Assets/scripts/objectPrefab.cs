using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Linq;

public class objectPrefab : MonoBehaviour
{
    string JsonDataString;
    private string OriginalJsonSite = "https://digitalcultures.ncl.ac.uk/projects/magicalreality/site/php/get_all_routes.php";

    public GameObject cluny;
    public GameObject cagedBird;
    public GameObject growUp;
    public GameObject mdhm;
    public GameObject hyestd;
    public GameObject clayCellFracture;
    public GameObject worldMusic;
    public GameObject BBeautiful;


    public Text debugText;
    DateTime now;

    GameObject Clone;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        debugText.text = Global.chosenRoute;

        WWW readingsite = new WWW(OriginalJsonSite);
        yield return readingsite;

        Debug.Log("TRY TO LOAD OBJECTS!");

        if (string.IsNullOrEmpty(readingsite.error))
        {
            JsonDataString = readingsite.text;
        }

        JSONNode users = SimpleJSON.JSON.Parse("{\n\"users\": " + JsonDataString + "}");


        foreach (var user in users["users"])
        {
            // UserNames.text += kpv.Key.ToString().ToUpper();

            Debug.Log(user.Value["user"].ToString());

            if (Global.userid == user.Value["user"].ToString().Substring(1, user.Value["user"].ToString().Length - 2))
            {

                foreach (var routesJSON in user.Value["routes"])
                {
                    Debug.Log(routesJSON.Value["routeName"].ToString() + "  route:" + Global.chosenRoute.ToString());

                    //Debug.Log(Global.chosenRoute.ToString());

                    if (routesJSON.Value["routeName"].ToString().ToUpper() == Global.chosenRoute.ToString())
                    {
                        var i = 0;
                        foreach (var path in routesJSON.Value["path"])
                        {
                            // CountryName.text = jsonNode["country"].ToString().ToUpper();
                            // Debug.Log(jsonNode["country"]);
                            // Debug.Log(key + " = " + key["latitude"].ToString());
                            //  Debug.Log(kpv.Value["_id"]);
                            Debug.Log(path.Value["pointId"]);
                            Debug.Log(path.Value["lat"]);
                            Debug.Log(path.Value["lon"]);
                            var model = path.Value["model"].ToString();
                            model = model.Substring(1, model.Length - 2);

                            Debug.Log(path.Value["model"] + "  model:" + model);

                            // 1 = cluny
                            if (model == "cluny.png")
                            {
                                Debug.Log("cluny!!!!!!!!!!!");
                                Clone = Instantiate(cluny, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 2 = cagedBird
                            if (model == "bird.png")
                            {
                                Clone = Instantiate(cagedBird, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 3 = growUp
                            if (model == "growup.png")
                            {
                                Clone = Instantiate(growUp, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 4 = mdhm
                            if (model == "murder.png")
                            {
                                Clone = Instantiate(mdhm, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 5 = hyestd
                            if (model == "dead.png")
                            {
                                Clone = Instantiate(hyestd, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 6 = clayCellFracture
                            if (model == "clay.png")
                            {
                                Clone = Instantiate(clayCellFracture, transform.position, Quaternion.identity) as GameObject;
                            }

                            // 7 = worldMusic
                            if (model == "world.png")
                            {
                                Clone = Instantiate(worldMusic, transform.position, Quaternion.identity) as GameObject;
                            }
                            else

                            // 8 = clayCellFracture
                            if (model == "beautiful.png")
                            {
                                Clone = Instantiate(BBeautiful, transform.position, Quaternion.identity) as GameObject;
                            }


                            if (Clone != null)
                            {
                                //set position
                                Clone.transform.parent = GameObject.FindWithTag("worldRoot").transform;

                                var scriptReference = Clone.GetComponent<ObjectPlacement>();
                                if (scriptReference != null)
                                {
                                    scriptReference.SetLonLat(path.Value["lon"], path.Value["lat"]);

                                    //for debug
                                    Global.objectNames.Add(model);
                                    Global.objectDistance.Add(0);
                                    Global.objectAlpha.Add(0);
                                    scriptReference.SetID(i);
                                    i++;

                                }

                                //set poi mask
                                var scriptReferencePOI = Clone.GetComponent<poi>();
                                if (scriptReferencePOI != null)
                                {
                                    scriptReferencePOI.SetUiContainert(GameObject.FindWithTag("mask"));
                                }


                            }

                            debugText.text = debugText.text + Environment.NewLine + path.Value["model"] +"\t"+ path.Value["action"];

                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //debug
        now = DateTime.Now;
        debugText.text = now.Second.ToString();
        for (var i = 0; i < Global.objectDistance.Count; i++)
        {
            debugText.text = debugText.text + Environment.NewLine + Global.objectNames.ElementAt(i)+ "\t" + Global.objectDistance.ElementAt(i) + "\t" + Global.objectAlpha.ElementAt(i);
        }
        debugText.text = debugText.text + Environment.NewLine + Global.debug;

    }
}