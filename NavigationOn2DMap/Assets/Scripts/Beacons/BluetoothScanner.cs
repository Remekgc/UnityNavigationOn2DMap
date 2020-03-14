using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class BluetoothScanner : Scanner
{
    public TextMeshProUGUI bluetoothScanText;
    public bool updateUI = true;

    void Start()
    {
        Beacons = new List<Beacon>();
        JavaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        JavaObject.Call("setBluetoothScanReceiver");
        InvokeRepeating("UpdateUIText", 5, 1);
        StartCoroutine(Scan());
    }

    public override IEnumerator Scan()
    {
        while (true)
        {
            if (ScanEnabled)
            {
                JavaObject.Call("startBluetoothScan");
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
            bluetoothScanText.text = "";
            foreach (var item in Beacons)
            {
                bluetoothScanText.text += item.SSID + " : " + item.RSSI + "\n";
            }
        }
    }
}
