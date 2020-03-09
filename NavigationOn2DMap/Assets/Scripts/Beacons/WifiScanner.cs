using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public sealed class WifiScanner : Scanner
{
    public int scanFrequency = 31;
    public string filter;
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

    public void UpdateUIText()
    {
        wifiScanText.text = "";
        if (updateUI)
        {
            foreach (var item in Beacons)
            {
                wifiScanText.text += item.SSID + " : " + item.RSSI + "\n";
            }
        }
    }

    public override void StopScan()
    {
        throw new System.NotImplementedException();
    }
}
