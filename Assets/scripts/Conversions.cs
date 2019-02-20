using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Conversions{
	private const int earthRadius=6378137; //Meters
	///<summary>
	/// Calculates the Haversine distance in m between two points
	///Vector.x is Lon and y is Lat
	///</summary>
	///<param name=lat0> float: Latitude of the first point </param>
	///<param name=lon0> float: Longitude of the first point </param>
	///<param name=lat1> float: Latitude of the second point </param>
	///<param name=lon1> float: Longitude of the second point </param>
	///<returns>float distance between points in meters</param>
	public static float coordsDistance(float lat0,float lon0, float lat1, float lon1){
		float deltaLat=(lat1-lat0)*Mathf.Deg2Rad;
		float deltaLon=(lon1-lon0)*Mathf.Deg2Rad;
		float lat0Rad=lat0*Mathf.Deg2Rad;
		float lat1Rad=lat1*Mathf.Deg2Rad;
		float haversine=Mathf.Pow(Mathf.Sin(deltaLat/2),2)
			+Mathf.Cos(lat0Rad)*Mathf.Cos(lat1Rad)
			*Mathf.Pow(Mathf.Sin(deltaLon),2);
	    float angDist= 2*Mathf.Atan2(Mathf.Sqrt(haversine), Mathf.Sqrt(1-haversine));
	    float distance=earthRadius*angDist;
		return distance;

	}

	public static float coordsDistance(Vector2 point0, Vector2 point1){
		float distance=coordsDistance(point0.y,point0.x,point1.y,point1.x);
		return distance;
	}

	public static double coordsDistance(dLocation point0, dLocation point1){
		double distance=coordsDistance(point0.lat,point0.lon,point1.lat,point1.lon);
		return distance;
	}

	public static double coordsDistance(double lat0,double lon0, double lat1, double lon1){
		double deg2Rad=System.Math.PI/180;
		double deltaLat=(lat1-lat0)*deg2Rad;
		double deltaLon=(lon1-lon0)*deg2Rad;
		double lat0Rad=lat0*deg2Rad;
		double lat1Rad=lat1*deg2Rad;
		double haversine=System.Math.Pow(System.Math.Sin(deltaLat/2),2)
			+System.Math.Cos(lat0Rad)*System.Math.Cos(lat1Rad)
			*System.Math.Pow(System.Math.Sin(deltaLon),2);
	    double angDist= 2*System.Math.Atan2(System.Math.Sqrt(haversine), System.Math.Sqrt(1-haversine));
	    double distance=earthRadius*angDist;
		return distance;

	}



	///<summary>
	/// Calculates the heading (relative to North) between two points
	//in degrees.
	///</summary>
	///<param name=lat0> float: Latitude of the first point </param>
	///<param name=lon0> float: Longitude of the first point </param>
	///<param name=lat1> float: Latitude of the second point </param>
	///<param name=lon1> float: Longitude of the second point </param>
	///<returns>float: Heading in degress</param>
	public static float bearing(float lat0,float lon0, float lat1, float lon1){
		float deltaLon=(lon1-lon0)*Mathf.Deg2Rad;
		float lat0Rad=lat0*Mathf.Deg2Rad;
		float lat1Rad=lat1*Mathf.Deg2Rad;
		float y = Mathf.Sin(deltaLon)*Mathf.Cos(lat1Rad);
		float x= Mathf.Cos(lat0Rad)*Mathf.Sin(lat1Rad)-
			Mathf.Sin(lat0Rad)*Mathf.Cos(lat1Rad)*Mathf.Cos(deltaLon);
		float heading= Mathf.Atan2(y,x);
		return (360+heading*Mathf.Rad2Deg)%360;
	}

	public static float bearing(Vector2 point0, Vector2 point1){
		float heading=bearing(point0.y,point0.x,point1.y, point1.x);
		return heading;
	}

	public static float bearing(double lat0,double lon0, double lat1, double lon1){
		double deg2Rad=System.Math.PI/180;
		double deltaLon=(lon1-lon0)*deg2Rad;
		double lat0Rad=lat0*deg2Rad;
		double lat1Rad=lat1*deg2Rad;
		double y = System.Math.Sin(deltaLon)*System.Math.Cos(lat1Rad);
		double x= System.Math.Cos(lat0Rad)*System.Math.Sin(lat1Rad)-
			System.Math.Sin(lat0Rad)*System.Math.Cos(lat1Rad)*System.Math.Cos(deltaLon);
		double heading= System.Math.Atan2(y,x);
		return (float)(360-(heading*Mathf.Rad2Deg))%360;
	}

	public static float bearing(dLocation point0, dLocation point1){
		double heading=bearing(point0.lat,point0.lon,point1.lat, point1.lon);
		return (float)heading;
	}
	public static Vector2 metersPerLat(float lat){
		Vector2 metersLonLat= new Vector2();
		float m1 = 111132.92f;    // latitude calculation term 1
	    float m2 = -559.82f;        // latitude calculation term 2
	    float m3 = 1.175f;      // latitude calculation term 3
	    float m4 = -0.0023f;        // latitude calculation term 4
	    float p1 = 111412.84f;    // longitude calculation term 1
	    float p2 = -93.5f;      // longitude calculation term 2
	    float p3 = 0.118f;      // longitude calculation term 3
	    lat = lat * Mathf.Deg2Rad;
	
	    // Calculate the length of a degree of latitude and longitude in meters
	    //m per Lat
	    metersLonLat.y = Mathf.Abs(-m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat)));
		//m per Lon
		metersLonLat.x =Mathf.Abs( (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat))); 
		return(metersLonLat);
	}

	public static Vector2 metersPerLat(double lat){
		Vector2 metersLonLat= new Vector2();
		double m1 = 111132.92f;    // latitude calculation term 1
	    double m2 = -559.82f;        // latitude calculation term 2
	    double m3 = 1.175f;      // latitude calculation term 3
	    double m4 = -0.0023f;        // latitude calculation term 4
	    double p1 = 111412.84f;    // longitude calculation term 1
	    double p2 = -93.5f;      // longitude calculation term 2
	    double p3 = 0.118f;      // longitude calculation term 3
	    lat = lat * Mathf.Deg2Rad;
	
	    // Calculate the length of a degree of latitude and longitude in meters
	    //m per Lat
	    metersLonLat.y = (float) System.Math.Abs(-m1 + 
	    	(m2 * System.Math.Cos((2 * lat))) + 
    		(m3 * System.Math.Cos(4 * lat)) + 
    		(m4 * System.Math.Cos(6 * lat))
    		);
		//m per Lon
		metersLonLat.x = (float) System.Math.Abs(
			(p1 * System.Math.Cos(lat)) + 
			(p2 * System.Math.Cos(3 * lat))+
			(p3 * System.Math.Cos(5 * lat))
			); 
		return(metersLonLat);
	}

	///<summary>
	/// Calculates the x distance between two points in meters
	///Vector.x is Lon and y is Lat
	///</summary>
	///<param name=lat0> float: Latitude of the first point </param>
	///<param name=lon0> float: Longitude of the first point </param>
	///<param name=lat1> float: Latitude of the second point </param>
	///<param name=lon1> float: Longitude of the second point </param>
	///<returns>float: "horizontal" distance between points in meters</param>
	public static float deltaX(float lat0,float lon0, float lat1, float lon1){
		float distance=coordsDistance(lat0,lon0,lat0,lon1);
		return distance;
	}

	///<summary>
	/// Calculates the x distance between two points in meters
	///Vector.x is Lon and y is Lat.
	///</summary>
	///<param name=point0> Vector2: (Longitude, Latitude) of the first point </param>
	///<param name=point1> Vector2: (Longitude, Latitude) of the first point </param>
	///<returns>float: "horizontal" distance between points in meters</param>
	public static float deltaX(Vector2 point0, Vector2 point1){
		float distance= coordsDistance(point0.y,point0.x,point1.y,point0.x);
		return distance;
	}

	///<summary>
	/// Calculates the Y distance between two points in meters
	///Vector.x is Lon and y is Lat
	///</summary>
	///<param name=lat0> float: Latitude of the first point </param>
	///<param name=lon0> float: Longitude of the first point </param>
	///<param name=lat1> float: Latitude of the second point </param>
	///<param name=lon1> float: Longitude of the second point </param>
	///<returns>float: "horizontal" distance between points in meters</param>
	public static float deltaY(float lat0,float lon0, float lat1, float lon1){
		float distance=coordsDistance(lat0,lon0,lat1,lon0);
		return distance;
	}

	///<summary>
	/// Calculates the Y distance between two points in meters
	///Vector.x is Lon and y is Lat.
	///</summary>
	///<param name=point0> Vector2: (Longitude, Latitude) of the first point </param>
	///<param name=point1> Vector2: (Longitude, Latitude) of the first point </param>
	///<returns>float: "horizontal" distance between points in meters</param>
	public static float deltaY(Vector2 point0, Vector2 point1){
		float distance= coordsDistance(point0.y,point0.x,point0.y,point1.x);
		return distance;
	}

	///<summary>
	/// Calculates the X,Y coordinates (in meters) from an origin to a point 
	///</summary>
	///<param name=origin> Vector2: (Longitude, Latitude) of the origin </param>
	///<param name=point> Vector2: (Longitude, Latitude) of the first point </param>
	///<returns>Vector2: x and y distance from origin to point</param>
	public static Vector3 lonLatToXZ(Vector2 origin, Vector2 point){
		Vector3 xz = new Vector3();
		Vector2 metersLonLat=metersPerLat(point.y);
		xz.z = metersLonLat.y*(point.y-origin.y);
		xz.y=-1;
		xz.x = metersLonLat.x*(point.x-origin.x);
		return xz;

	}

	public static Vector3 lonLatToXZ(dLocation origin, dLocation point){
		Vector3 xz = new Vector3();
		Vector2 metersLonLat=metersPerLat(point.lat);
		xz.z = (float)(metersLonLat.y*(point.lat-origin.lat));
		xz.y=-1;
		xz.x = (float)(metersLonLat.x*(point.lon-origin.lon));
		return xz;

	}

	public static Vector3 lonLatToXZ(Vector2 origin, double lon, double lat){
		Vector3 xz = new Vector3();
		Vector2 metersLonLat=metersPerLat(lat);
		xz.z = metersLonLat.y*((float)(lat-origin.y));
		xz.y=-1;
		xz.x = metersLonLat.x*((float)(lon-origin.x));
		return xz;
	}

	public static Vector3 rotatePoint(Vector3 point, double angle){
		Vector3 output= new Vector3();
		/*
		output.x=point.x*Mathf.Cos((float)angle)+point.z*Mathf.Sin((float)angle);
		output.z=-point.x*Mathf.Sin((float)angle)+point.z*Mathf.Cos((float)angle);
		*/
		output.x=point.x*Mathf.Cos((float)-angle)-point.z*Mathf.Sin((float)-angle);
		output.z=point.x*Mathf.Sin((float)-angle)+point.z*Mathf.Cos((float)-angle);
		
		output.y=point.y;
		return output;
	}

	public static float xzDistance(Vector3 position1, Vector3 position2){
		float distance;
		distance = Mathf.Sqrt(Mathf.Pow(position1.x-position2.x, 2)
				+Mathf.Pow(position1.z-position2.z, 2));
		return distance;
	}

}
