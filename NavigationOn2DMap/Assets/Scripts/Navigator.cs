using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Navigator : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private protected TextMeshProUGUI beaconListTextField;

    [Header("Navigator Components")]
    [SerializeField] private protected ComponentLoader componentLoader;
    [SerializeField] private protected WifiScanner wifiScanner;
    [SerializeField] private protected MapController mapController;
    public MapBeacon closestBeacon = null;

    void Awake()
    {
        wifiScanner.functionsToRunAfterScan.Add(UpdateDotPositionBasedOnSignalStrength);
    }

    void UpdateDotPositionBasedOnSignalStrength()
    {
        if (componentLoader.setupReady && wifiScanner.Beacons.Count > 0 && componentLoader.building.Beacons.Count > 0)
        {
            SortBeacons();

            if (closestBeacon != null)
            {
                mapController.PlaceDot(closestBeacon.MapPos.x, closestBeacon.MapPos.y);
                beaconListTextField.text = closestBeacon.RoomName;
            }
        }
        else if (componentLoader.setupReady && wifiScanner.Beacons.Count == 0)
        {
            mapController.RemoveDot();
            beaconListTextField.text = "No beacons around you found.";
        }
    }

    private void SortBeacons()
    {
        closestBeacon = null;

        foreach (var mapBeacon in componentLoader.building.Beacons)
        {
            foreach (var beacon in wifiScanner.Beacons)
            {
                beaconListTextField.text = mapBeacon.SSID + " != " + beacon.SSID;
                if (mapBeacon.SSID == beacon.SSID && closestBeacon == null)
                {
                    updateClosestBeacon(mapBeacon, beacon);
                }
                else if (mapBeacon.SSID == beacon.SSID && beacon.RSSI < closestBeacon.RSSI)
                {
                    updateClosestBeacon(mapBeacon, beacon);
                }
            }
        }
    }

    private void updateClosestBeacon(MapBeacon mapBeacon, Beacon beacon)
    {
        closestBeacon = mapBeacon;
        closestBeacon.RSSI = beacon.RSSI;
    }
}
