using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconSensor
{
    public string Name;
    public string Value;

    public BeaconSensor(string name = "None", string value = "")
    {
        Name = name;
        Value = value;
    }

}
