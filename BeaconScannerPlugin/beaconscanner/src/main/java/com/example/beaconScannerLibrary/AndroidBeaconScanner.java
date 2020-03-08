package com.example.beaconScannerLibrary;

import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiManager;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.util.List;

public class AndroidBeaconScanner {
    private Context context = UnityPlayer.currentActivity.getApplicationContext(); // Getting app context that is assigned in Unity class.
    private WifiManager wifiManager = (WifiManager) context.getApplicationContext().getSystemService(Context.WIFI_SERVICE); // Getting Wifi service.
    private String TAG = "UnityBeaconPlugin"; // Tag for Logcat
    private String UnityBeaconScannerObject = "BeaconScannerPlugin"; // Game object that we call in Unity - it searches it for the class with functions that are called from here.
    private IntentFilter intentFilter = new IntentFilter(); // new Intent Filter
    private BluetoothManager bluetoothManager = (BluetoothManager) context.getApplicationContext().getSystemService(Context.BLUETOOTH_SERVICE); // Getting Bluetooth service.

    private BroadcastReceiver wifiScanReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context c, Intent intent) {
            boolean success = intent.getBooleanExtra(WifiManager.EXTRA_RESULTS_UPDATED, false);
            if (success) {
                List<ScanResult> results = wifiManager.getScanResults();
                Log.d(TAG, "Wifi Scan Successful");
                String wifiScanResult = "";
                for (ScanResult result:results) {
                    wifiScanResult += "{SSID : " + result.SSID + "} - {RSSI : " + result.level + "}\n";
                }
                UnityPlayer.UnitySendMessage(UnityBeaconScannerObject, "SetWifiManagerText", wifiScanResult);
            } else {
                List<ScanResult> results = wifiManager.getScanResults();
                Log.d(TAG, "Wifi Scan failed");
                String wifiScan = "";
                for (ScanResult result:results) {
                    wifiScan += "{SSID : " + result.SSID + "} - {RSSI : " + result.level + "}\n";
                }
                UnityPlayer.UnitySendMessage(UnityBeaconScannerObject, "SetWifiManagerText", wifiScan);
            }
        }
    };

    public void setWifiScanReceiver(){
        // Setting up WiFi
        intentFilter.addAction(WifiManager.SCAN_RESULTS_AVAILABLE_ACTION);
        context.registerReceiver(wifiScanReceiver, intentFilter);
        // Setting up Bluetooth
        intentFilter.addAction(BluetoothDevice.ACTION_FOUND);
        context.registerReceiver(bluetoothScanReceiver, intentFilter);
        Log.d(TAG, "Scanner started started");
    }

    public void startWifiScan(){
        wifiManager.startScan();
    }

    public void startBluetoothScan(){
        bluetoothManager.getAdapter().startDiscovery();
    }

    private final BroadcastReceiver bluetoothScanReceiver = new BroadcastReceiver() {
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (BluetoothDevice.ACTION_FOUND.equals(action)) {
                // Discovery has found a device. Get the BluetoothDevice
                // object and its info from the Intent.
                BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
                int rssi = intent.getShortExtra(BluetoothDevice.EXTRA_RSSI,Short.MIN_VALUE);
                assert device != null;
                Log.d(TAG, "Bluetooth Device name: " + device.getName() + ", RSSI: " + rssi);
                UnityPlayer.UnitySendMessage(UnityBeaconScannerObject, "SetBlueToothManagerText", "{Name : " + device.getName() + "} - {RSSI : " + rssi + "}");
            }
        }
    };

    public void LogSentFromUnity(String unityLog){
        Log.d(TAG, unityLog);
    }

    public String SendDataToUnity(String textFromUnity){
        return textFromUnity + "Hello Unity, I'm Android\n";
    }

    public void callUnityFunctionWithoutParameter(){
        // make call to Unity function
        UnityPlayer.UnitySendMessage(UnityBeaconScannerObject, "testUnityFunctionFromAndroid", "");
    }

    public void callUnityFunctionWithParameter(String unityLog){
        // make call to Unity function
        UnityPlayer.UnitySendMessage(UnityBeaconScannerObject, "testUnityFunctionFromAndroid2", unityLog);
    }
}
