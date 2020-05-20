using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBeacon
{
    public int ID;
    public string RoomName;
    public string SSID;
    public Vector2 mapPos;
    public List<BeaconSensor> sensors = new List<BeaconSensor>();

    public MapBeacon(int id, string roomName, Vector2 pos, string ssid)
    {
        ID = id;
        RoomName = roomName;
        SSID = ssid;
        mapPos = pos;
    }

}
