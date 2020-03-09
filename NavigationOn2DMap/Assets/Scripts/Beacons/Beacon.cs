using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Beacon : IBeacon
{
    public string SSID { get; set; }
    public int RSSI { get; set; }

    public Beacon(string ssid = "None", int rssi = 0)
    {
        SSID = ssid;
        RSSI = rssi;
    }

}
