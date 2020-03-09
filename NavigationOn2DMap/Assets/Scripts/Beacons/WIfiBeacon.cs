using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WifiBeacon : Beacon
{
    // Won't be used unless we create direct connection to the beacons.
    public WifiBeacon(string ssid = "emptyWifiBeacon", int rssi = 0)
    {
        SSID = ssid;
        RSSI = rssi;
    }
}
