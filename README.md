# forcesensor

Needs drivers from https://www.phidgets.com/docs/Phidgets_Drivers

Calibration software for getting m and b values available from

https://www.phidgets.com/?tier=3&catid=98&pcid=78&prodid=1027


# Arduino
- Creates access point
- http://192.168.4.1/ provides UI for 
- http://192.168.4.1/RL turn red led off
- http://192.168.4.1/LAUNCH turns red led on ( does it still work after this, perhaps not currently?)

- http://192.168.4.1/STATUS returns millis() - arduino function (eg. Arduino uptime in milliseconds), used for poll loop of 1 sec for connection check to Arduino
