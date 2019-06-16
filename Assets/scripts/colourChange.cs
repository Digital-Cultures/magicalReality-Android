using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class colourChange : MonoBehaviour
{
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    ColorGrading m_colour;

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
    }

    void Update()
    {
        Reset();

        foreach (KeyValuePair<string, Global.Effect> effect in Global.EffectsApllied)
        {
            //if (effect.Value == Global.Effect.None)
            //{
            //    
            //}

            if (effect.Value == Global.Effect.Vignette)
            {
                Debug.Log("UPDATE COLOUR Vignette "+ Mathf.Sin(Time.realtimeSinceStartup));
                m_Vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);

                break;
            }

            else if(effect.Value == Global.Effect.Blue)
            {
                Debug.Log("UPDATE COLOUR Blue "+ (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f);

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -100) + 100f;

                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -100) + 100f;


                break;
            }

            else if(effect.Value == Global.Effect.Red)
            {
                Debug.Log("UPDATE COLOUR Red "+ (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f);

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup)* -100)+100f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f;

                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -100) + 100f;


                break;
            }

            else if (effect.Value == Global.Effect.Green)
            {
                Debug.Log("UPDATE COLOUR Red "+ (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f);

                m_colour.mixerBlueOutBlueIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -100) + 100f;

                m_colour.mixerRedOutRedIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * -100) + 100f;
  
                m_colour.mixerGreenOutGreenIn.value = (Mathf.Sin(Time.realtimeSinceStartup) * 100) + 100f;

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
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}
