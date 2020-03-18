using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BluetoothBeacon : Beacon
{
    public BluetoothBeacon(string ssid = "emptyBluetoothBeacon", int rssi = 0)
    {
        SSID = ssid;
        RSSI = rssi;
    }
}
