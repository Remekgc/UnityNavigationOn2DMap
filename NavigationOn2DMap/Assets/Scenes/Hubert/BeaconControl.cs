using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconControl : MonoBehaviour
{
    public GameObject map;
    public GameObject beaconIcon;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        GameObject beacon = Instantiate(beaconIcon);
        beacon.transform.position = new Vector3(x, y, 2); //cos zjebane z tym 2d albo warstwami



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
