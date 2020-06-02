﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using System;

public sealed class WifiScanner : Scanner
{
    public TextMeshProUGUI WifiScanText;
    public TextMeshProUGUI WifiCounter;
    public bool UpdateUI = false;
    public List<Action> functionsToRunAfterScan = new List<Action>();

    void Awake()
    {
        Beacons = new List<Beacon>();
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setWifiScanReceiver");
    }

    void Start()
    {
        StartCoroutine(Scan());
    }

    public override IEnumerator Scan()
    {
        while (true)
        {
            if (ScanEnabled)
            {
                Beacons.Clear(); // Clear the beacon list

                JavaObject.Call("startWifiScan");
                yield return new WaitForSeconds(ScanFrequency);

                int t = int.Parse(WifiCounter.text);
                t++;
                WifiCounter.text = t.ToString();

                UpdateUIText();
                runAfterScanFunctions();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void UpdateUIText()
    {
        if (UpdateUI)
        {
            WifiScanText.text = "";
            foreach (var beacon in Beacons)
            {
                WifiScanText.text += beacon.SSID + " : " + beacon.RSSI + "\n";
            }
        }
    }

    private void runAfterScanFunctions()
    {
        foreach (var function in functionsToRunAfterScan)
        {
            function();
        }
    }
}
