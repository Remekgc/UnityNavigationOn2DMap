using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingComponents : MonoBehaviour
{
     /* TODO: Here we are going to test the functionality and do everything step by step
     * 1: Get the GPS Data and select everything by it
     * 2: Download the images based on links from the database
     * 3: Load the image into the scene as a map with all beacons and show where we are based on wifi scan
     */

    [Header("GPS")]
    [SerializeField] private protected GPS gps;
    [SerializeField] float lat, log;
    [SerializeField] bool gpsLoading, gpsReady = false;
    [Header("SQL")]
    [SerializeField] private protected SQLManager sqlManager;
    [SerializeField] bool sqlLoading, sqlReady = false;
    [SerializeField] private protected Building building;
    [Header("Image Loader")]
    [SerializeField] private protected ImportFloorMaps floorMapsImporter; // Pavlo: Change to ImageManager or somethin
    [SerializeField] bool imageLoading, imageReady = false;
    //Pavlo: ImageLoader imageloader;

    void Start()
    {
        LoadNavigation();
    }

    public void LoadNavigation()
    {
        gpsReady = false;
        sqlReady = false;
        imageReady = false;
        StartCoroutine(ILoadNavigationComponents());
    }

    private IEnumerator ILoadNavigationComponents()
    {
        bool navigationLoaded = false;
        while (!navigationLoaded)
        {
            // Check every state
            if (!gpsReady)
            {
                if (!gpsLoading)
                {
                    StartCoroutine(IGetGPSData());
                    gpsLoading = true;
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                else
                {
                    print("Gps Loading");
                    yield return new WaitForSeconds(1f);
                    continue;
                }
            }
            else if (!sqlReady)
            {
                if (!sqlLoading)
                {
                    StartCoroutine(IGetDatabaseData(lat, log));
                    sqlLoading = true;
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                else
                {
                    print("SQL Loading");
                    yield return new WaitForSeconds(1f);
                    continue;
                }
            }
            else if (!imageReady)
            {
                if (!imageLoading)
                {
                    StartCoroutine(floorMapsImporter.DownloadImage(building.ImageLink, building.Name, building.ID));
                    print("Image Loaded");
                    imageLoading = true;
                    imageReady = true;
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                else
                {
                    print("Image loading...");
                    yield return new WaitForSeconds(1f);
                    continue;
                }
            }
            navigationLoaded = true;
        }
    }

    private IEnumerator IGetGPSData()
    {
        Debug.Log("IGetGPSData started");
        //gps.getLatLog()
        while (true)
        {
            if (true) //if gps.locatingFinished
            {
                // lat = gps.getLatitude()
                // log = gps.getLongitude()
                lat = 51.27f;
                log = 22.55f;
                gpsReady = true;
                yield break;
            }
            else
            {
                print("Getting data");
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private IEnumerator IGetDatabaseData(float lat, float log)
    {
        /* Adrian
         * In here we have to create the building object
         * building has ID, buildingName, imageLink and list of beacons of type MapBeacon so
         * 1. Get the building ID, name and image link with GPS data(lat, log)
         * 2. Get the List of Beacons with the building ID, MapBeacon object type has ID, RoomName, SSID, Vector2 MapPos that is X and Y in the database and List of Sensors (BeaconSensor type) - Done
         * 3. Get List of Sensors for the MapBeacon, BeaconSensor has name and value - Done
         * 4. Create the building object - Done
         */
        

        Debug.Log("IGetDatabaseData started");
        //sqlManager.ExecuteReaderQuery("SELECT Name FROM Building WHERE Name = 'Adrian_Home'");
        //sqlManager.ExecuteReaderQuery("SELECT * FROM Building");

        bool buildingDataReady = false;
        sqlManager.ExecuteReaderQuery("SELECT * FROM Building WHERE Name = 'Adrian_Home'");

        // Getting building data
        while (!buildingDataReady)
        {
            if (sqlManager.SelectQueryDone)
            {
                List<string> data = new List<string>();

                foreach (var x in sqlManager.selectQueryResult)
                {
                    foreach (var y in x)
                    {
                        data.Add(y);
                    }
                }
                sqlManager.SelectQueryDone = false;

                building = new Building(int.Parse(data[0]), data[1], data[2]);

                buildingDataReady = true;
            }
            else
            {
                print("Waiting for the query to complete!");
                yield return new WaitForSeconds(2);
            }
        }

        bool mapBeaconsDataReady = false;
        sqlManager.ExecuteReaderQuery("SELECT * FROM Beacon WHERE BuildingID = " + building.ID);

        while (!mapBeaconsDataReady)
        {
            if (sqlManager.SelectQueryDone)
            {
                List<List<string>> data = new List<List<string>>();
                int dataid = 0;

                foreach (var x in sqlManager.selectQueryResult)
                {
                    data.Add(new List<string>());
                    foreach (var y in x)
                    {
                        data[dataid].Add(y);
                        print(y);
                    }
                    dataid++;
                }
                sqlManager.SelectQueryDone = false;
                dataid = 0;

                foreach (var x in data)
                {
                    building.Beacons.Add(
                        new MapBeacon(
                            int.Parse(data[dataid][0]),
                            data[dataid][1],
                            new Vector2(int.Parse(data[dataid][2]), int.Parse(data[dataid][3])),
                            data[dataid][4])
                        );
                    dataid++;
                }

                mapBeaconsDataReady = true;
            }
            else
            {
                print("Waiting for the query to complete!");
                yield return new WaitForSeconds(2);
            }
        }

        
        foreach (var beacon in building.Beacons)
        {
            bool beaconSensorsReady = false;
            sqlManager.ExecuteReaderQuery("SELECT * FROM Sensor WHERE BeaconID = " + beacon.ID);

            while (!beaconSensorsReady)
            {
                if (sqlManager.SelectQueryDone)
                {
                    List<List<string>> data = new List<List<string>>();
                    int dataid = 0;

                    foreach (var x in sqlManager.selectQueryResult)
                    {
                        data.Add(new List<string>());
                        foreach (var y in x)
                        {
                            data[dataid].Add(y);
                            print(y);
                        }
                        dataid++;
                    }

                    foreach (var x in data)
                    {
                        beacon.sensors.Add(new BeaconSensor(x[1], float.Parse(x[2])));
                    }

                    sqlManager.SelectQueryDone = false;
                    beaconSensorsReady = true;
                }
                else
                {
                    print("Waiting for the query to complete!");
                    yield return new WaitForSeconds(2);
                }
            }
        }

        print("welp it worked XDDDD");
        sqlReady = true;
        yield break;
    }
}
