using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text coordinates;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coordinates.text = "Lat:" + GPS.Instance.latitude.ToString() + "    Lon:" + GPS.Instance.longitude.ToString();
    }
}
