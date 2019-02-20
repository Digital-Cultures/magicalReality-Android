//
//  GPS.m
//  GPS
//
//  Created by MaxBotvinev on 23.06.2018.
//  Copyright © 2018 MaxBotvinev. All rights reserved.
//

#import "GPS.h"

@implementation GPS

CLLocationManager *locationManager;

+ (GPS*)instance
{
    static GPS *instance = nil;
    if( !instance )
        instance = [[GPS alloc] init];
    return instance;
}

- (void)start {
    
    //NSLog(@"START GPS");
    locationManager = [[CLLocationManager alloc] init];
    locationManager.desiredAccuracy = kCLLocationAccuracyBest;
    
    if ([locationManager respondsToSelector:@selector(requestWhenInUseAuthorization)]) {
        
        [locationManager requestWhenInUseAuthorization];
    }
    
    [locationManager startUpdatingLocation];
}

- (void) askPermission {
    
    UIAlertView * alertView =
    [[UIAlertView alloc] initWithTitle:@"Location Services Disabled!"
                               message:@"Please enable Location Based Services for better results! We promise to keep your location private"
                              delegate:self
                     cancelButtonTitle:@"Settings"
                     otherButtonTitles:@"Cancel", nil];
    
    [alertView show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == 0)
    {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
    }
    if (buttonIndex == 1)
    {
        [self askPermission];
    }
}

- (BOOL) isEnabledByUser {
    
    return [CLLocationManager locationServicesEnabled] && [CLLocationManager authorizationStatus] == kCLAuthorizationStatusAuthorizedWhenInUse;
}

- (double)altitude {
    
    return locationManager.location.altitude;
}

- (double)latitude {
    
    //NSLog(@" lat: %.100g",locationManager.location.coordinate.latitude);
    return locationManager.location.coordinate.latitude;
}

- (double)longitude {
    
    //NSLog(@" lon: %.100g",locationManager.location.coordinate.longitude);
    return locationManager.location.coordinate.longitude;
}

- (double)horizontalAccuracy {
    
    return locationManager.location.horizontalAccuracy;
}

- (double)verticalAccuracy {
    
    return locationManager.location.verticalAccuracy;
}

- (double)timestamp {
    
    return locationManager.location.timestamp.timeIntervalSince1970;
}

- (void)stop {
    
    //NSLog(@"STOP GPS");
    [locationManager stopUpdatingLocation];
}

@end

#ifdef __cplusplus
extern "C" {
#endif
    
    //Interface
    //------------------------------
    
    void start() {
        
        [[GPS instance] start];
    }
    
    void askPermission() {
        
        [[GPS instance] askPermission];
    }
    
    bool isEnabledByUser() {
        
        return [[GPS instance] isEnabledByUser];
    }
    
    double altitude() {
        
        return [[GPS instance] altitude];
    }
    
    double latitude() {
        
        return [[GPS instance] latitude];
    }
    
    double longitude() {
        
        return [[GPS instance] longitude];
    }
    
    double horizontalAccuracy() {
        
        return [[GPS instance] horizontalAccuracy];
    }
    
    double verticalAccuracy() {
        
        return [[GPS instance] verticalAccuracy];
    }
    
    double timestamp() {
        
        return [[GPS instance] timestamp];
    }
    
    
    void stop() {
        
        [[GPS instance] stop];
    }
    
    //------------------------------
    
#ifdef __cplusplus
}
#endif
