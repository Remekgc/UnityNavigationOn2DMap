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

    void RefreshBeaconData()
    {
        if (navigator.closestBeacon != null)
        {
            sensorText.text = "Step 1\n";
            StartCoroutine(IGetSensorDataFromDatabase());
        }
    }

    IEnumerator IGetSensorDataFromDatabase()
    {
        sensorText.text = "Step 2(Beacon ID:" + navigator.closestBeacon.ID + ")\n";
        sqlManager.ExecuteReaderQuery("SELECT * FROM Sensor WHERE BeaconID = " + navigator.closestBeacon.ID);

        while (sqlManager.selectQueryDone)
        {
            yield return new WaitForSeconds(1f); // Wait unitill the select query is done.
        }

        UpdateSenorValues();
        UpdateUI_SensorText();
    }

    private void UpdateSenorValues()
    {
        sensorText.text = "Step 3(Select query list size " + sqlManager.selectQueryResult.Count + ")\n";
        for (int i = 0; i < sqlManager.selectQueryResult.Count; i++)
        {
            //navigator.closestBeacon.Sensors[i].Value = float.Parse(sqlManager.selectQueryResult[i][2]);
            sensorText.text += "Step 3.1, Sensor val:" + sqlManager.selectQueryResult[i][2] + "\n";
        }
    }

    private void UpdateUI_SensorText()
    {
        sensorText.text = "Step 4\n";
        sensorText.text = " ";
        foreach (var sensor in navigator.closestBeacon.Sensors)
        {
            sensorText.text += sensor.Name + ": " + sensor.Value + "\n";
        }
    }
}
