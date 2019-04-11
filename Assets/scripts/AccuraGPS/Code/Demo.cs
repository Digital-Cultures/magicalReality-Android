using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AccuraGPS.Demo
{
	public class Demo : MonoBehaviour
	{            
		public Text
            coords,
    		latitudeText,
    		longtitudeText,
		    altitudeText,
    		horizontalAccuracyText,
    		verticalAccuracyText,
    		timestampText,
		    messageText;

		public Animator copyMessage;      
		WaitForSeconds interval = new WaitForSeconds(2.0f);
		const string PRECISION_QUALIFIER = "G17";

		IEnumerator Start()
		{
			do //Start Gps
			{
				GPS.Start();
				yield return interval;
			}
			while (!GPS.IsEnabledByUser);

            //Processing...
            while(GPS.IsEnabledByUser)
			{
				PrintPreciseCoords();
                PrintPrecise(altitudeText, GPS.Altitude);
                PrintPrecise(latitudeText, GPS.Latitude);
                PrintPrecise(longtitudeText, GPS.Longitude);
                PrintPrecise(horizontalAccuracyText, GPS.HorizontalAccuracy);
                PrintPrecise(verticalAccuracyText, GPS.VerticalAccuracy);
                PrintPrecise(timestampText, GPS.Timestamp);
                yield return interval;
			}
		}
              
		void PrintPrecise(Text lable, double value)
		{
			lable.text = value.ToString(PRECISION_QUALIFIER);
		}      

		void PrintPreciseCoords()
        {
			coords.text = string.Format(
				"X: {0}\nY: {1}", 
				GPS.Latitude.ToString(PRECISION_QUALIFIER), 
				GPS.Longitude.ToString(PRECISION_QUALIFIER));
        }  

		public void CopyCoordinates()
		{
			UniClipboard.SetText(
				string.Format(
					"{0}, {1}", 
					latitudeText.text, 
					longtitudeText.text));
			
			ShowMessage("Coordinates copied to clipboard");
		}

		public void ShowMessage(string message)
		{
			copyMessage.SetTrigger("show");
			messageText.text = message;
		}

		void OnDestroy()
		{
			//Stop GPS
			if(GPS.IsEnabledByUser)
			{
				GPS.Stop();
			}
		}
	}
}