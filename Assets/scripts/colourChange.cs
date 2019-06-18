using System.Collections;
using System.Collections.Generic;
using GetSocialSdk.Capture.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

public class colourChange : MonoBehaviour
{
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    ColorGrading m_colour;

    public GetSocialCapturePreview capturePreview; 
    public GetSocialCapture capture;

    public GameObject gifPreview;
    public GameObject recordBtn;
    public GameObject uploadBtn;

    private static string uploadURL = "https://magicalreality.ncl.ac.uk/php/upload.php";

    private bool startCapture = false;
    private byte[] myData;

    void Start()
    {
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(0f);

        m_colour = ScriptableObject.CreateInstance<ColorGrading>();
        m_colour.enabled.Override(true);
       
        m_colour.mixerBlueOutBlueIn.Override(150f);
        m_colour.mixerRedOutBlueIn.Override(0f);
        m_colour.mixerGreenOutBlueIn.Override(0f);

        m_colour.mixerBlueOutRedIn.Override(0f);
        m_colour.mixerRedOutRedIn.Override(150f);
        m_colour.mixerGreenOutRedIn.Override(0f);

        m_colour.mixerBlueOutGreenIn.Override(0f);
        m_colour.mixerRedOutGreenIn.Override(0f);
        m_colour.mixerGreenOutGreenIn.Override(150f);

        m_colour.saturation.Override(100f);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_colour);


        //
        recordBtn.SetActive(false);
        gifPreview.SetActive(false);
        uploadBtn.SetActive(false);

    }

    void Update()
    {
        Reset();

        foreach (KeyValuePair<string, Global.Effect> effect in Global.EffectsApllied)
        {
            if (effect.Value != Global.Effect.None)
            {
                if (!startCapture)
                {
                    recordBtn.SetActive(true);
                }

            }

            if (effect.Value == Global.Effect.Vignette)
            {
                Debug.Log("UPDATE COLOUR Vignette "+ Mathf.Sin(Time.realtimeSinceStartup));
                m_Vignette.intensity.value = (Mathf.Sin(Time.realtimeSinceStartup)+1)/2;

                break;
            }

            else if(effect.Value == Global.Effect.Blue)
            {
                Debug.Log("UPDATE COLOUR Blue "+ ((Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f));

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -50) + 50f;

                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -50) + 50f;


                break;
            }

            else if(effect.Value == Global.Effect.Red)
            {
                Debug.Log("UPDATE COLOUR Red "+ ((Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f));

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup)* - 50)+50f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f;

                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -50) + 50f;


                break;
            }

            else if (effect.Value == Global.Effect.Green)
            {
                Debug.Log("UPDATE COLOUR GREEN "+ ((Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f));

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -50) + 50f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -50) + 50f;
  
                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 50) + 50f;

                break;
            }

            else if (effect.Value == Global.Effect.BandW)
            {
                Debug.Log("UPDATE COLOUR B&W : "+Mathf.Sin(Time.realtimeSinceStartup) * 100);

                m_colour.saturation.value = Mathf.Sin(Time.realtimeSinceStartup) * 100;

                break;
            }

            else if (effect.Value == Global.Effect.Hue)
            {
                Debug.Log("UPDATE COLOUR Hue "+ Mathf.Sin(Time.realtimeSinceStartup) * 300);

                m_colour.hueShift.value = Mathf.Sin(Time.realtimeSinceStartup) * 300;

                break;
            }
        }
    }

    public void Capture()
    {
        startCapture = true;
        capture.StartCapture(Global.userid+"_"+ Global.chosenRoute.Substring(1, Global.chosenRoute.Length - 2) + "_"+ Global.pointID);
        Invoke("ActionFinished", 4);
        recordBtn.SetActive(false);
    }

    public void Upload()
    {
        Debug.Log("Upload File... Todo:");
        StartCoroutine(UploadGiftoSeverver());
        //send source file in url
        gifPreview.SetActive(false);
        uploadBtn.SetActive(false);
    }

    public void Cancel()
    {
        gifPreview.SetActive(false);
        uploadBtn.SetActive(false);
    }


    // stop recording
    private void ActionFinished()
    {
        capture.StopCapture();
        capture.GenerateCapture(result =>
        {
            // use gif, like send it to your friends by using GetSocial Sdk
            Debug.Log("Should save gif");
            //upload to somwhere
        });

        // show preview
        gifPreview.SetActive(true);
        uploadBtn.SetActive(true);
        capturePreview.Play();

        startCapture = false;
    }

    private void Reset()
    {
        m_Vignette.intensity.Override(0f); 

        m_colour.mixerBlueOutBlueIn.Override(150f);
        m_colour.mixerRedOutBlueIn.Override(0f);
        m_colour.mixerGreenOutBlueIn.Override(0f);

        m_colour.mixerBlueOutRedIn.Override(0f);
        m_colour.mixerRedOutRedIn.Override(150f);
        m_colour.mixerGreenOutRedIn.Override(0f);

        m_colour.mixerBlueOutGreenIn.Override(0f);
        m_colour.mixerRedOutGreenIn.Override(0f);
        m_colour.mixerGreenOutGreenIn.Override(150f);

        m_colour.saturation.Override(100f);
        m_colour.hueShift.Override(0f);

        recordBtn.SetActive(false);
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }

    IEnumerator UploadGiftoSeverver()
    {
        Debug.Log("Upload started : "+ capture.GetFilePath()+"   "+  Application.persistentDataPath);


        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("fileToUpload", System.IO.File.ReadAllBytes(capture.GetFilePath()), capture.GetFileName(), "image/gif"));

        UnityWebRequest www = UnityWebRequest.Post(uploadURL, formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!"+www.responseCode+"  "+www.downloadHandler.text);
        }
    }
}
