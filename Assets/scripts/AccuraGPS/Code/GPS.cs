using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AccuraGPS
{
	public static class GPS
	{
#if UNITY_IOS && !UNITY_EDITOR
		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("__Internal")]
		static extern void start();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
        static extern void askPermission();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
		static extern bool isEnabledByUser();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
		static extern double altitude();

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("__Internal")]
		static extern double latitude();

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("__Internal")]
		static extern double longitude();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
		static extern double horizontalAccuracy();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
		static extern double verticalAccuracy();

		[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("__Internal")]
		static extern double timestamp();

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("__Internal")]
		static extern void stop();
#elif UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject gpsClass = null;
        private static AndroidJavaObject activityContext = null;
#else
        const string NOT_SUPPORTED_WARNING = "This platform is not supported.";
#endif

        public static void Start()
        {
#if UNITY_IOS && !UNITY_EDITOR
			start();
#elif UNITY_ANDROID && !UNITY_EDITOR
            //For Old Androids
			using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
    			if(version.GetStatic<int>("SDK_INT") < 23)
    			{
			        AskPermission();
    			}
            }
            //First Time
            if (gpsClass == null)
            {
                using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    activityContext = activity.Call<AndroidJavaObject>("getApplicationContext");
                }
                using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.giganeo.accuragpsjava.GPS"))
                {
                    if (pluginClass != null)
                    {
                        gpsClass = pluginClass.CallStatic<AndroidJavaObject>("instance");
                        gpsClass.Call("setContext", activityContext);
                    }
                }
            }
            gpsClass.Call("start");
#else
            Debug.LogWarning(NOT_SUPPORTED_WARNING);
#endif
        }

		public static void AskPermission()
		{
#if UNITY_IOS && !UNITY_EDITOR
			askPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
			Input.location.Start();
			Input.location.Stop();
#else
            Debug.LogWarning(NOT_SUPPORTED_WARNING);
#endif         
		}
      
		public static bool IsEnabledByUser
		{
			get
            {
#if UNITY_IOS && !UNITY_EDITOR
			    return isEnabledByUser();
#elif UNITY_ANDROID && !UNITY_EDITOR
                if(gpsClass != null)
                    return gpsClass.Call<bool>("isEnabledByUser");
                return false;
#else
                return false;
#endif
            }
		}

		public static double Altitude
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return altitude(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return gpsClass.Call<double>("altitude");
#else
                return 0;
#endif
            }
        }

		public static double Latitude
		{
			get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return latitude(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return gpsClass.Call<double>("latitude");
#else
                return 0;
#endif
            }
		}

		public static double Longitude
		{
			get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return longitude(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return gpsClass.Call<double>("longitude");
#else
                return 0;
#endif
            }
		}

		public static double HorizontalAccuracy
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return horizontalAccuracy(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return gpsClass.Call<double>("horizontalAccuracy");
#else
                return 0;
#endif
            }
        }

		public static double VerticalAccuracy
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return verticalAccuracy(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return gpsClass.Call<double>("verticalAccuracy");
#else
                return 0;
#endif
            }
        }

		public static double Timestamp
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
				return timestamp(); 
#elif UNITY_ANDROID && !UNITY_EDITOR
                //gpsClass.Call("start");
                return (double)gpsClass.Call<long>("timestamp");
#else
                return 0;
#endif
            }
        }

        public static void Stop()
        {
#if UNITY_IOS && !UNITY_EDITOR
			stop();
#elif UNITY_ANDROID && !UNITY_EDITOR
            gpsClass.Call("stop");
#endif
        }
    }
}