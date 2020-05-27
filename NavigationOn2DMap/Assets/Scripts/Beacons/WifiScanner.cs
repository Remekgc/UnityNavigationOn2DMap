using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public sealed class WifiScanner : Scanner
{
    public TextMeshProUGUI wifiScanText;
    public bool updateUI = true;

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
                JavaObject.Call("startWifiScan");
                yield return new WaitForSeconds(ScanFrequency);
                UpdateUIText();
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
