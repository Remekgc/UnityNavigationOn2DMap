using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SensorDataManager : MonoBehaviour
{
    [Header("Requierd Components")]
    [SerializeField] private protected ComponentLoader componentLoader;
    [SerializeField] private protected SQLManager sqlManager;
    [SerializeField] private protected WifiScanner wifiScanner;
    [SerializeField] private protected TextMeshProUGUI sensorText;
    [SerializeField] private protected Navigator navigator;

    void Awake()
    {
        wifiScanner.functionsToRunAfterScan.Add(RefreshBeaconData);
    }

    void Start()
    {
        StartCoroutine(UpdateUI_SensorText());
    }

    void RefreshBeaconData()
    {
        if (navigator.closestBeacon != null)
        {
            StartCoroutine(IGetSensorDataFromDatabase());
        }
    }

    IEnumerator IGetSensorDataFromDatabase()
    {
        sqlManager.ExecuteReaderQuery("SELECT * FROM Sensor WHERE BeaconID = " + navigator.closestBeacon.ID);

        while (!sqlManager.selectQueryDone)
        {
            yield return new WaitForSeconds(2f); // Wait unitill the select query is done.
            Debug.Log("Waiting for sensor data to refresh");
        }

        UpdateSenorValues();
    }

    private void UpdateSenorValues()
    {
        for (int i = 0; i < sqlManager.selectQueryResult.Count; i++)
        {
            navigator.closestBeacon.Sensors[i].Value = sqlManager.selectQueryResult[i][2];
        }
    }

    IEnumerator UpdateUI_SensorText()
    {
        while (true)
        {
            if (navigator.closestBeacon != null)
            {
                sensorText.text = " ";
                foreach (var sensor in navigator.closestBeacon.Sensors)
                {
                    sensorText.text += sensor.Name + ": " + sensor.Value + "\n";
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
