using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testBeaconScanner : MonoBehaviour
{
    public TextMeshProUGUI wifiScanText;
    public TextMeshProUGUI bluetoothScanText;

    private AndroidJavaObject javaObject;
    WifiScanner wifiScanner;

    void Start()
    {
        javaObject = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        javaObject.Call("setWifiScanReceiver");
        InvokeRepeating("CallWifiScan", 1, 31);
        InvokeRepeating("CallBluetoothScan", 1, 12);
        //javaClass.Call("LogSentFromUnity", "Test Unity -> Android communication");
        //wifiManagerText.text = javaClass.Call<string>("SendDataToUnity", "Hello Android, I'm Unity\n");
        // Calling java function that calls Unity function - for example, without any reason.
        //javaClass.Call("callUnityFunctionWithoutParameter");
        //javaClass.Call("callUnityFunctionWithParameter", "Hello there, I'm General Kenobi\n");
    }

    public void StopScan()
    {
        //wifiScanner.StopScan();
        wifiScanText.text = "Scanner stopped";
    }

    public void CallWifiScan()
    {
        javaObject.Call("startWifiScan");
    }

    public void CallBluetoothScan()
    {
        bluetoothScanText.text = "";
        javaObject.Call("startBluetoothScan");
    }

    public void SetWifiManagerText(string wifiList)
    {
        wifiScanText.text = wifiList;
    }

    public void SetBluetoothManagerText(string bluetoothDevice)
    {
        bluetoothScanText.text += bluetoothDevice + "\n";
    }

    public void TestUnityFunctionFromAndroid()
    {
        wifiScanText.color = Color.black;
    }

    public void TestUnityFunctionFromAndroid2(string msg)
    {
        wifiScanText.text += msg;
    }
}

