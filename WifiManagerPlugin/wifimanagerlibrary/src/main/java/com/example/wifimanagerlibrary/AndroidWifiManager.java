package com.example.wifimanagerlibrary;

import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class AndroidWifiManager {

    public void LogWifiManager(){
        Log.d("UnityWifiManager", "Hello from wifi manager");
        Log.d("UnityWifiManager", "Welp");
    }

    public void LogSentFromUnity(String unityLog){
        Log.d("UnityWifiManager", unityLog);
    }

    public String SendDataToUnity(String textFromUnity){
        return textFromUnity + "Hello Unity, I'm Android\n";
    }

    public void callUnityFunctionWithoutParameter(){
        // make call to Unity function
        UnityPlayer.UnitySendMessage("WifiManagerPlugin", "testUnityFunctionFromAndroid", "");
    }

    public void callUnityFunctionWithParameter(String unityLog){
        // make call to Unity function
        UnityPlayer.UnitySendMessage("WifiManagerPlugin", "testUnityFunctionFromAndroid2", unityLog);
    }
}
