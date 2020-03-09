using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class BluetoothScanner : Scanner
{
    /*public BluetoothScanner(int scanFrequency = 12, string filter = "none")
    {
        ScanFrequency = scanFrequency;
        Filter = filter;
        Beacons = new List<Beacon>();
        BeaconListRecentlyUpdated = false;
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
    }*/

    public int scanFrequency = 12;
    public string filter = "none";
    public TextMeshProUGUI bluetoothScanText;
    public bool updateUI = true;

    void Start()
    {
        ScanFrequency = scanFrequency;
        Filter = filter;
        Beacons = new List<Beacon>();
        BeaconListRecentlyUpdated = false;
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setBluetoothScanReceiver");
        InvokeRepeating("updateUIText", 5, 1);
        InvokeRepeating("StartScan", 1, ScanFrequency);
    }

    public override void StartScan()
    {
        JavaObject.Call("startBluetoothScan");
    }

    public override void StartScan(string filter)
    {
        JavaObject.Call("startBluetoothScan", filter);
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
        bluetoothScanText.text = "";
        if (updateUI)
        {
            foreach (var item in Beacons)
            {
                bluetoothScanText.text += item.SSID + " - " + item.RSSI + "\n";
            }
        }
    }
}
