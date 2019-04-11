package com.GigaNeo.accuragpsjava;

import android.content.Context;


public class GPS {

    private Context context;
    private static GPS instance;

    private SimpleLocation location;

    private GPS(){
        instance = this;
    }

    public static GPS instance(){
        if(instance == null){
            instance = new GPS();
        }
        return instance;
    }

    public void setContext(Context context) {
        this.context = context;
    }

    public boolean isEnabledByUser(){
        // if we can't access the location yet
        if (!location.hasLocationEnabled()) {
            // ask the user to enable location access
            SimpleLocation.openSettings(context);
        }

        return location.hasLocationEnabled();
    }

    public void start(){
        // construct a new instance of SimpleLocation
        location = new SimpleLocation(context, true, false, 2000, true);

        // if we can't access the location yet
        if (!location.hasLocationEnabled()) {
            // ask the user to enable location access
            SimpleLocation.openSettings(context);
        }

        location.beginUpdates();
    }

    public double latitude() {
        return location.getLatitude();
    }

    public double longitude() {
        return  location.getLongitude();
    }

    public double altitude() {
        return location.getAltitude();
    }

    public double horizontalAccuracy() {
        return location.getHorizontalAccuracy();
    }

    public double verticalAccuracy() {
        return location.getVerticalAccuracy();
    }

    public long timestamp() {
        return  location.getTimestamp();
    }

    public void stop() {
        location.endUpdates();
    }
}
