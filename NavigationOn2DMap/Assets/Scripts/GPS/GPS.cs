using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public static GPS Instance { set; get; }
    public double latitude;
    public double longitude;
    public float updateDistance = 10f;
    public string buildingLatitude;
    public string buildingLongitude;
    private string oldBuildingLatitude, oldBuildingLongitude;
    public List<Point> points;
    public SimpleSQLUserScriptExample sqlData;
    [SerializeField] protected SQLManager sqlManager;
    private Point coordinates;

    private List<double> distanceToBuilding;

    void Start()
    {
        latitude = 51.27639;
        longitude = 22.551178;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(StartLocationService());
        GetDatabaseData();

    }
    private void Update()
    {
        //Debug.Log("Count: "+sqlManager.selectQueryResult.Count());
        if (sqlManager.SelectQueryDone)
        {
            //Debug.Log("Test: " + sqlManager.selectQueryResult[0][3]);
            if (buildingLatitude == ""&& buildingLongitude != sqlManager.selectQueryResult[0][3])
                buildingLatitude = sqlManager.selectQueryResult[0][3];
            else if (buildingLongitude == "" && buildingLatitude != sqlManager.selectQueryResult[0][4])
                buildingLongitude = sqlManager.selectQueryResult[0][4];
        }
        if (buildingLongitude != buildingLatitude && buildingLongitude != "" && buildingLatitude != "" && oldBuildingLatitude != buildingLatitude && oldBuildingLongitude != buildingLongitude)
        {
            oldBuildingLatitude = buildingLatitude;
            oldBuildingLongitude = buildingLongitude;
            coordinates = new Point(latitude, longitude);
            var latitudeList = buildingLatitude.Split(';').Select(double.Parse).ToList();
            List<double> longitudeList = new List<double>(Array.ConvertAll(buildingLongitude.Split(';'), double.Parse));

            List<Point> distanceBuildingPlayer = new List<Point>();

            distanceBuildingPlayer.Add(new Point(((latitudeList[0] + latitudeList[1]) / 2) - latitude, ((longitudeList[0] + longitudeList[1]) / 2) - longitude));

           // Debug.Log(distanceBuildingPlayer[0].latitude+"/"+distanceBuildingPlayer[0].longitude);

            double distance =0;
            for(int i = 0; i<distanceBuildingPlayer.Count; i++)
            {
                if (distanceBuildingPlayer[i].latitude > distance)
                    distance = distanceBuildingPlayer[i].latitude;
            }

           // Debug.Log(latitudeList[0]);
            //Debug.Log(longitudeList[0]);
            List<Point> points = new List<Point>();
            for (int i = 0; i < latitudeList.Count; i++)
            {
                points.Add(new Point(latitudeList[i], longitudeList[i]));
            }
           // Debug.Log(points.Count);
            //if(points.Count>=4)
               // Debug.Log(IsInsideBuilding(points, coordinates));
        }
    }

    public void GetDatabaseData()
    {
        StartCoroutine(IGetDatabaseData());
    }

    public IEnumerator IGetDatabaseData()
    {
        double minLatitude, maxLatitude, minLongitude, maxLongitude;
        minLatitude = latitude - 0.02;
        maxLatitude = latitude + 0.02;
        minLongitude = longitude - 0.02;
        maxLongitude = longitude + 0.02;
        Debug.Log("IGetDatabaseData started");
        // tu se wstaw query jakie chcesz
        //sqlManager.ExecuteReaderQuery("SELECT Name FROM Building WHERE Name = 'Adrian_Home'");
        //sqlManager.ExecuteReaderQuery("SELECT * FROM Building");
        sqlManager.ExecuteReaderQuery("SELECT * FROM Building WHERE Latitude BETWEEN '"+minLatitude+"' AND '"+maxLatitude+"' AND Longitude BETWEEN '"+minLongitude+"' AND '"+maxLongitude+"'");
        while (true)
        {
            if (sqlManager.SelectQueryDone)
            {
                foreach (var x in sqlManager.selectQueryResult)
                {
                    foreach (var y in x)
                    {
                        print(y);
                    }
                }
                sqlManager.SelectQueryDone = false;
                yield break;
            }
            else
            {
                print("Waiting for the query to complete!");
                yield return new WaitForSecondsRealtime(2);
            }
        }

    }

    //tego nie ruszac bo zabije
    private bool IsInsideBuilding(List<Point> polygon, Point coordinates)
    {
        bool result = false;
        int j = polygon.Count() - 1;
        for(int i = 0; i < polygon.Count(); i++)
        {
            if(polygon[i].latitude < coordinates.latitude && polygon[j].latitude >= coordinates.latitude
                || polygon[j].latitude < coordinates.latitude && polygon[i].latitude >= coordinates.latitude)
            {
                if(polygon[i].longitude + (coordinates.latitude - polygon[i].latitude) / (polygon[j].latitude - polygon[i].latitude)*(polygon[j].longitude - polygon[i].longitude) < coordinates.longitude)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }
    //tego tez >.>
    private IEnumerator StartLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }

        Input.location.Start(updateDistance);
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if(maxWait <= 0)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }


        yield break;
    }

}
