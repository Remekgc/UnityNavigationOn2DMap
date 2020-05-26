using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public int ID;
    public string Name, ImageLink;
    public List<MapBeacon> Beacons = new List<MapBeacon>();
    public Vector2 Coordinates = new Vector2();

    public Building(int id, string name, string imageLink, Vector2 coordinates)
    {
        ID = id;
        Name = name;
        ImageLink = imageLink;
        Coordinates = coordinates;
    }

}
