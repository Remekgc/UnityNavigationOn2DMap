using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testWifiPlugin : MonoBehaviour
{

    private AndroidJavaObject javaClass;

    // Start is called before the first frame update
    void Start()
    {
        javaClass = new AndroidJavaObject("com.example.wifimanagerlibrary.AndroidWifiManager");
        javaClass.Call("LogWifiManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
