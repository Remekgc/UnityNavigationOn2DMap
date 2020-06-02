using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Scanner : MonoBehaviour, IScanner
{
    public AndroidJavaObject JavaObject;
    public string Filter;
    public int ScanFrequency;
    public List<Beacon> Beacons;
    public bool ScanEnabled = true;

    public void EnableScan()
    {
        ScanEnabled = true;
    }

    public void DisableScan()
    {
        ScanEnabled = false;
    }

    public abstract IEnumerator Scan();

    //Does not have to be overriden but if we decide to add some functionality based on connection type then it will have to be.
    public virtual void UpdateBeaconList(string scanResult)
    {
        // Filter the data
        int rssi = 0;
        string ssid, temp = "";

        for (int i = 0; i < scanResult.Length; i++)
        {
            if (scanResult[i] != '~')
            {
                temp += scanResult[i];
            }
            else
            {
                rssi = Mathf.Abs(int.Parse(temp));
                temp = "";
            }
        }
        ssid = temp;
        Beacons.Add(new Beacon(ssid, rssi));

        // Filter the list
        //if (Beacons.Count > 0 && (ssid.Contains(Filter) || Filter == ""))
        //{
        //    for (int i = 0; i < Beacons.Count; i++)
        //    {
        //        if (Beacons[i].SSID == ssid)
        //        {
        //            if (Beacons[i].RSSI != rssi)
        //            {
        //                Beacons[i].RSSI = rssi;
        //            }
        //            break;
        //        }
        //        else if (i == Beacons.Count - 1)
        //        {
        //            Beacons.Add(new Beacon(ssid, rssi));
        //        }
        //    }
        //}
        //else if(ssid.Contains(Filter) || Filter == "")
        //{
        //    Beacons.Add(new Beacon(ssid, rssi));
        //}
    }

}
