using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeacon
{
    string SSID { get; set; }
    int RSSI { get; set; }
}
