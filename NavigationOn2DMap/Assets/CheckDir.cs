using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CheckDir : MonoBehaviour
{
    string appPath;
    string test;
    [SerializeField] GameObject text;
    void Start()
    {
        appPath = Path.GetDirectoryName(Application.persistentDataPath);
        test = Path.Combine(appPath, "WSEI");
        Directory.CreateDirectory(test);
        
        text.GetComponent<Text>().text =  test;
    }

}
