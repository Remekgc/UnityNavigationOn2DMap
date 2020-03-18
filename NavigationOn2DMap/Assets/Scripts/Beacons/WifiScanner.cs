﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public sealed class WifiScanner : Scanner
{
    public TextMeshProUGUI wifiScanText;
    public bool updateUI = true;

    void Start()
    {
        Beacons = new List<Beacon>();
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setWifiScanReceiver");
        InvokeRepeating("UpdateUIText", 5, 1);
        StartCoroutine(Scan());
    }

    public override IEnumerator Scan()
    {
        while (true)
        {
            if (ScanEnabled)
            {
                JavaObject.Call("startWifiScan");
                yield return new WaitForSeconds(ScanFrequency);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void UpdateUIText()
    {
        if (updateUI)
        {
            wifiScanText.text = "";
            foreach (var item in Beacons)
            {
                wifiScanText.text += item.SSID + " : " + item.RSSI + "\n";
            }
        }
    }
}