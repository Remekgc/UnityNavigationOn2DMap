using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testBeaconScanner : MonoBehaviour
{
    public TextMeshProUGUI wifiScanText;
    public TextMeshProUGUI bluetoothScanText;

    private AndroidJavaObject javaClass;

    void Start()
    {
        javaClass = new AndroidJavaObject("com.example.beaconScannerLibrary.AndroidBeaconScanner");
        javaClass.Call("setWifiScanReceiver");
        InvokeRepeating("CallWifiScan", 1, 31);
        InvokeRepeating("CallBluetoothScan", 1, 12);
        //javaClass.Call("LogSentFromUnity", "Test Unity -> Android communication");
        //wifiManagerText.text = javaClass.Call<string>("SendDataToUnity", "Hello Android, I'm Unity\n");
        // Calling java function that calls Unity function - for example, without any reason.
        //javaClass.Call("callUnityFunctionWithoutParameter");
        //javaClass.Call("callUnityFunctionWithParameter", "Hello there, I'm General Kenobi\n");
    }

    public void CallWifiScan()
    {
        javaClass.Call("startWifiScan");
    }

    public void CallBluetoothScan()
    {
        javaClass.Call("startBluetoothScan");
    }

    public void SetWifiManagerText(string wifiList)
    {
        wifiScanText.text = wifiList;
    }

    public void SetBlueToothManagerText(string bluetoothList)
    {
        bluetoothScanText.text = "";
        bluetoothScanText.text += bluetoothList + "\n";
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

