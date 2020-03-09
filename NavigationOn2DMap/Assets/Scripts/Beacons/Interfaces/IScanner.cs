using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public interface IScanner
{
    // Parameters
    AndroidJavaObject JavaObject { get; set; }
    Thread scanThread { get; set; }
    string Filter { get; set; }
    int ScanFrequency { get; set; }
    List<Beacon> Beacons { get; set; }
    bool BeaconListRecentlyUpdated { get; set; }
    // Functioncs
    void StartScan();
    void StartScan(string filter);
    void StopScan();
    void UpdateBeaconList(string ssid, int rssi);
    
}
