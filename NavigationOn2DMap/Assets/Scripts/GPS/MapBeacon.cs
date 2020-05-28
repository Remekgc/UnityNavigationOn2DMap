using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBeacon
{
    public int ID;
    public string RoomName;
    public string SSID;
    public int RSSI;
    public Vector2 MapPos;
    public List<BeaconSensor> Sensors = new List<BeaconSensor>();

    public MapBeacon(int id, string roomName, Vector2 mapPos, string ssid)
    {
        ID = id;
        RoomName = roomName;
        SSID = ssid;
        MapPos = mapPos;
    }

}
