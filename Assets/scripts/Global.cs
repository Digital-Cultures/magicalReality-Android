using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Global{

	//Origin and bearing control
	public static bool originSet =false;
	public static Vector2 origin= new Vector2(0,0);
	public static dLocation dOrigin= new dLocation(0,0);
	public static bool bearingSet=false;
	public static bool tutorialComplete=false;
	public static bool sampleShown=false;
	public static bool placeTutorialComplete=false;
	public static dLocation dPlayerLatLon=new dLocation(0,0);
	//Region and model placement options
	public static float planeY=0;
	public static bool showGrid=false;
	public static int activeSet=0;
	public static int ActiveSet {
	   get{return activeSet;}
	   set{activeSet=value;} }
	public static int activeModel=0;
	public static dLocation region1 = new dLocation(-1.592145f,54.97465f);
	public const int rows=4;
	public const int cols=6;
	public static string[,] prefabPaths=new string[rows,cols]{ 
		{ "cluny", "mdhm", "growUp","","",""}, 
		{ "mdhm", "growUp", "BBeautiful","","",""},
        { "growUp", "BBeautiful","worldMusic","","",""},
        { "BBeautiful","worldMusic","growUp","mdhm","cagedBird","hyestd"}
    };
    //Player to model distance dictionary
    public static Dictionary<string,float> playerDistance=new Dictionary<string,float>();
    //Interval calculation controls
    public static uint opacityInterval=5;

    public static string chosenRoute = "";
    public static string userid = "";
    public static string walkid = "";
    public static List<string> objectNames = new List<string>();
    public static List<float> objectDistance = new List<float>();
    public static List<float> objectAlpha = new List<float>();
    public static string debug = "";
}