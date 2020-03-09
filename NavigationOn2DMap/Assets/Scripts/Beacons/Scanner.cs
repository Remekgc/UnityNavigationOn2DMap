using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Scanner : MonoBehaviour, IScanner
{
    public AndroidJavaObject JavaObject { get; set; }
    public Thread scanThread { get; set; }
    public string Filter { get; set; }
    public int ScanFrequency { get; set; }
    public List<Beacon> Beacons { get; set; }
    public bool BeaconListRecentlyUpdated { get; set; }

    /*public Scanner(int scanFrequency = 31, string filter = "none")
    {
        ScanFrequency = scanFrequency;
        Filter = filter;
        Beacons = new List<Beacon>();
        BeaconListRecentlyUpdated = false;
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
    }*/

    public abstract void StartScan();
    public abstract void StartScan(string filter);
    public abstract void StopScan();

    //Does not have to be overriden but if we decide to add some functionality based on connection type then it will have to be.
    public virtual void UpdateBeaconList(string ssid, int rssi)
    {
        if (Beacons.Count > 0)
        {
            for (int i = 0; i < Beacons.Count; i++)
            {
                if (Beacons[i].SSID == ssid)
                {
                    if (Beacons[i].RSSI != rssi)
                    {
                        Beacons[i].RSSI = rssi;
                        BeaconListRecentlyUpdated = true;
                    }
                    break;
                }
                else if (i == Beacons.Count - 1)
                {
                    Beacons.Add(new Beacon(ssid, rssi));
                }
            }
        }
        else
        {
            Beacons.Add(new Beacon(ssid, rssi));
        }

    }
}
