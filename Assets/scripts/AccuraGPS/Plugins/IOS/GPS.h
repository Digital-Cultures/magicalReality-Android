//
//  GPS.h
//  GPS
//
//  Created by MaxBotvinev on 23.06.2018.
//  Copyright © 2018 MaxBotvinev. All rights reserved.
//

#ifndef GPS_h
#define GPS_h
#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

@interface GPS : NSObject<UIAlertViewDelegate>

+ (GPS*)instance;
- (void)start;
- (void) askPermission;
- (BOOL) isEnabledByUser;
- (double)altitude;
- (double)latitude;
- (double)longitude;
- (double)horizontalAccuracy;
- (double)verticalAccuracy;
- (double)timestamp;
- (void)stop;

@end

#endif /* GPS_h */
