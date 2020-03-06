using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testWifiPlugin : MonoBehaviour
{
    public TextMeshProUGUI wifiManagerText;

    private AndroidJavaObject javaClass;

    void Start()
    {
        javaClass = new AndroidJavaObject("com.example.wifimanagerlibrary.AndroidWifiManager");
        javaClass.Call("setWifiScanReceiver");
        InvokeRepeating("CallStartWifiScan", 1, 31);
        //javaClass.Call("LogSentFromUnity", "Test Unity -> Android communication");
        //wifiManagerText.text = javaClass.Call<string>("SendDataToUnity", "Hello Android, I'm Unity\n");
        // Calling java function that calls Unity function - for example, without any reason.
        //javaClass.Call("callUnityFunctionWithoutParameter");
        //javaClass.Call("callUnityFunctionWithParameter", "Hello there, I'm General Kenobi\n");
    }

    public void CallStartWifiScan()
    {
        javaClass.Call("startWifiScan");
    }

    public void SetWifiManagerText(string wifiList)
    {
        wifiManagerText.text = wifiList;
    }

    public void TestUnityFunctionFromAndroid()
    {
        wifiManagerText.color = Color.black;
    }

    public void TestUnityFunctionFromAndroid2(string msg)
    {
        wifiManagerText.text += msg;
    }
}
