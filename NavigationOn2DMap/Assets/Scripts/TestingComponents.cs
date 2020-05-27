using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestingComponents : MonoBehaviour
{
    /* TODO: Here we are going to test the functionality and do everything step by step
    * 1: Get the GPS Data and select everything by it
    * 2: Download the images based on links from the database
    * 3: Load the image into the scene as a map with all beacons and show where we are based on wifi scan
    */
    [Header("Navigation Components")]
    public bool setupReady = false;

    [Header("GPS")]
    [SerializeField] float latitude;
    [SerializeField] float longitude;
    [SerializeField][Range(0.01f, 0.09f)][Tooltip("This controls how accurate the select from our database will be.")]
    float selectAccuracy = 0.02f;
    [SerializeField] bool gpsLoading, gpsReady = false;

    [Header("SQL")]
    [SerializeField] private protected SQLManager sqlManager;
    [SerializeField] bool sqlLoading, sqlReady = false;
    [SerializeField] private protected Building building;

    [Header("Image Loader")]
    [SerializeField] private protected ImportFloorMaps imageImporter;
    [SerializeField] List<Sprite> mapImages = new List<Sprite>();
    [SerializeField] private protected Image mapImage;
    [SerializeField] bool imageLoading, imageDownloaded = false;

    [Header("Map Object")]
    [SerializeField] private protected MapController mapController;
    [SerializeField] bool mapReady = false;

    void Start()
    {
        LoadNavigation();
    }

    public void LoadNavigation()
    {
        gpsReady = false;
        sqlReady = false;
        imageDownloaded = false;
        mapReady = false;
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
                }
                yield return new WaitForSeconds(1f);
                continue;
            }
            else if (!sqlReady)
            {
                if (!sqlLoading)
                {
                    StartCoroutine(IGetDatabaseData(latitude, longitude));
                    sqlLoading = true;
                }
                yield return new WaitForSeconds(1f);
                continue;
            }
            else if (!imageDownloaded && building != null)
            {
                if (!imageLoading)
                {
                    StartCoroutine(ILoadMapImage());
                    imageLoading = true;
                }
                yield return new WaitForSeconds(1f);
                continue;
            }
            navigationLoaded = true;
        }
    }

    private IEnumerator IGetGPSData()
    {
        print("IGetGPSData started");
        Point gpsPoint = GPS.Instance.getLocationPoint();

        while (true)
        {
            if (GPS.Instance.locatingFinished) //if gps.locatingFinished
            {
                latitude = (float)gpsPoint.latitude;
                longitude = (float)gpsPoint.longitude;
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
        print("IGetDatabaseData started");

        bool buildingDataReady = false;
        sqlManager.ExecuteReaderQuery("SELECT * FROM Building WHERE Latitude BETWEEN '" + (lat - selectAccuracy) + "' AND '" + (lat + selectAccuracy) + "' AND Longitude BETWEEN '" + (log - selectAccuracy) + "' AND '" + (log + selectAccuracy) + "'");

        // Getting building data
        while (!buildingDataReady && !sqlManager.emptyQueryResult)
        {
            if (sqlManager.selectQueryDone)
            {
                List<Building> buildingsToCompare = new List<Building>();
                foreach (var buildingData in sqlManager.selectQueryResult)
                {
                    buildingsToCompare.Add(new Building(
                        int.Parse(buildingData[0]),
                        buildingData[1],
                        buildingData[2],
                        new Vector2(float.Parse(buildingData[3]), float.Parse(buildingData[4])) // coordinates
                        ));
                }

                float dist = 0f;
                foreach (var b in buildingsToCompare)
                {
                    if (building == null)
                    {
                        building = b;
                        dist = Vector2.Distance(b.Coordinates, new Vector2(lat, log));
                    }
                    else if (Vector2.Distance(building.Coordinates, b.Coordinates) < dist)
                    {
                        building = b;
                    }
                }
                print(building.Name);

                sqlManager.selectQueryDone = false;
                buildingDataReady = true;
            }
            else
            {
                print("Waiting for the query to complete!");
                yield return new WaitForSeconds(1);
            }
        }

        bool mapBeaconsDataReady = false;
        if (building != null) sqlManager.ExecuteReaderQuery("SELECT * FROM Beacon WHERE BuildingID = " + building.ID);

        while (!mapBeaconsDataReady && !sqlManager.emptyQueryResult)
        {
            if (sqlManager.selectQueryDone)
            {
                foreach (var beaconData in sqlManager.selectQueryResult)
                {
                    building.Beacons.Add(
                        new MapBeacon(
                            int.Parse(beaconData[0]),
                            beaconData[1],
                            new Vector2(int.Parse(beaconData[2]), int.Parse(beaconData[3])),
                            beaconData[4])
                        );
                }

                mapBeaconsDataReady = true;
                sqlManager.selectQueryDone = false;
            }
            else
            {
                print("Waiting for the query to complete!");
                yield return new WaitForSeconds(1);
            }
        }

        if (!sqlManager.emptyQueryResult)
        {
            foreach (var beacon in building.Beacons)
            {
                bool beaconSensorsReady = false;
                sqlManager.ExecuteReaderQuery("SELECT * FROM Sensor WHERE BeaconID = " + beacon.ID);

                while (!beaconSensorsReady && !sqlManager.emptyQueryResult)
                {
                    if (sqlManager.selectQueryDone)
                    {
                        foreach (var sensorData in sqlManager.selectQueryResult)
                        {
                            beacon.Sensors.Add(new BeaconSensor(sensorData[1], float.Parse(sensorData[2])));
                        }

                        sqlManager.selectQueryDone = false;
                        beaconSensorsReady = true;
                    }
                    else
                    {
                        print("Waiting for the query to complete!");
                        yield return new WaitForSeconds(1);
                    }
                }
            }
        }

        print("SQL Ready");
        sqlReady = true;
        yield break;
    }

    IEnumerator ILoadMapImage()
    {
        print("ILoadMapImage started");

        imageImporter.listOfMapsLinks.Add(building.ImageLink);
        imageImporter.DownloadImageFromLink(building.ImageLink, building.ID, building.Name);

        while (!imageImporter.imageReady)
        {
            print("Loading Image...");
            yield return new WaitForSeconds(1f);
        }
        imageDownloaded = true;

        mapController.map.GetComponent<SpriteRenderer>().sprite = Sprite.Create(imageImporter.imageTexture, new Rect(x: 0, y: 0, imageImporter.imageTexture.width, imageImporter.imageTexture.height), new Vector2(x: 0.5f, y: 0.5f));
        mapReady = true;
    }
}
