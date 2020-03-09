using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class BluetoothScanner : Scanner
{
    public int scanFrequency = 12;
    public string filter;
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

    public void updateUIText()
    {
        bluetoothScanText.text = "";
        if (updateUI)
        {
            foreach (var item in Beacons)
            {
                bluetoothScanText.text += item.SSID + " : " + item.RSSI + "\n";
            }
        }
    }

    public override void StopScan()
    {
        throw new System.NotImplementedException();
    }
}
