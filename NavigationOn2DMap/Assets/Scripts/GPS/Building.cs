using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public int ID;
    public string Name, ImageLink;
    public List<MapBeacon> Beacons = new List<MapBeacon>();

    public Building(int id, string name, string imageLink)
    {
        ID = id;
        Name = name;
        ImageLink = imageLink;
    }

}
