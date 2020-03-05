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
        javaClass.Call("LogWifiManager");
        javaClass.Call("LogSentFromUnity", "Remi working here");
        wifiManagerText.text = javaClass.Call<string>("SendDataToUnity", "Hello Android, I'm Unity\n");
        // Calling java function that calls Unity function - for example, without any reason.
        javaClass.Call("callUnityFunctionWithoutParameter");
        javaClass.Call("callUnityFunctionWithParameter", "Hello there, I'm General Kenobi\n");
    }

    public void testUnityFunctionFromAndroid()
    {
        wifiManagerText.color = Color.black;
    }

    public void testUnityFunctionFromAndroid2(string msg)
    {
        wifiManagerText.text += msg;
    }
}
