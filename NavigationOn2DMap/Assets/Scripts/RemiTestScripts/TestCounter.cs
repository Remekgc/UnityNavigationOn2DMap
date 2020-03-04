using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public int counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void counterListener()
    {
        counter++;
        counterText.text = counter.ToString();
    }
}
