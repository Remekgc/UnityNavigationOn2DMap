using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconSensor
{
    public string Name;
    public float Value;

    public BeaconSensor(string name = "None", float value = 0f)
    {
        Name = name;
        Value = value;
    }

}
