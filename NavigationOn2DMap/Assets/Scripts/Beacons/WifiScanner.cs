using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public sealed class WifiScanner : Scanner
{
    /*public WifiScanner(int scanFrequency = 31, string filter = "none")
    {
        ScanFrequency = scanFrequency;
        Filter = filter;
        Beacons = new List<Beacon>();
        BeaconListRecentlyUpdated = false;
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setWifiScanReceiver");
    }*/

    public int scanFrequency = 31;
    public string filter = "none";
    public TextMeshProUGUI wifiScanText;
    public bool updateUI = true;

    void Start()
    {
        ScanFrequency = scanFrequency;
        Filter = filter;
        Beacons = new List<Beacon>();
        BeaconListRecentlyUpdated = false;
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setWifiScanReceiver");
        InvokeRepeating("updateUIText", 5, 1);
        InvokeRepeating("StartScan", 1, ScanFrequency);
    }

    public override void StartScan()
    {
        JavaObject.Call("startWifiScan");
    }

    public override void StartScan(string filter)
    {
        JavaObject.Call("startWifiScan", filter);
    }

    public override void StopScan()
    {
        if (scanThread != null && scanThread.IsAlive)
        {
            scanThread.Abort();
        }
    }

    public void UpdateBeaconList(string ssid)
    {
        int rssi = 0;
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

    public void updateUIText()
    {
        wifiScanText.text = "";
        if (updateUI)
        {
            foreach (var item in Beacons)
            {
                wifiScanText.text += item.SSID + " - " + item.RSSI + "\n";
            }
        }
    }
}
