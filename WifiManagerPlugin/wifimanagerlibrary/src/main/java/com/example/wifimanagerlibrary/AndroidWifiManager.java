package com.example.wifimanagerlibrary;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiManager;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.util.List;

public class AndroidWifiManager {
    private Context context = UnityPlayer.currentActivity.getApplicationContext();
    private WifiManager wifiManager = (WifiManager) context.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
    private String wifiScan = "", TAG = "UnityWifiManager";
    private IntentFilter intentFilter = new IntentFilter();

    private BroadcastReceiver wifiScanReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context c, Intent intent) {
            boolean success = intent.getBooleanExtra(WifiManager.EXTRA_RESULTS_UPDATED, false);
            if (success) {
                List<ScanResult> results = wifiManager.getScanResults();
                Log.d(TAG, "Scan Successful, new results:");
                //Log.d(TAG, results.toString());
                wifiScan = "";
                for (ScanResult result:results) {
                    wifiScan += "{SSID : " + result.SSID + "} - {RSSI : " + result.level + "}\n";
                }
            } else {
                // scan failure handling
                List<ScanResult> results = wifiManager.getScanResults();
                Log.d(TAG, "Scan failed, old results:");
                //Log.d(TAG, results.toString());
                wifiScan = "";
                for (ScanResult result:results) {
                    wifiScan += "{SSID : " + result.SSID + "} - {RSSI : " + result.level + "}\n";
                }
            }
        }
    };

    public void setWifiScanReceiver(){
        intentFilter.addAction(WifiManager.SCAN_RESULTS_AVAILABLE_ACTION);
        context.registerReceiver(wifiScanReceiver, intentFilter);
        Log.d(TAG, "Wifi manager started");
    }

    public String getWifiScan(){
        Log.d(TAG, "Trying to Scan");
        wifiManager.startScan();
        return wifiScan;
    }

    public void LogSentFromUnity(String unityLog){
        Log.d(TAG, unityLog);
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
