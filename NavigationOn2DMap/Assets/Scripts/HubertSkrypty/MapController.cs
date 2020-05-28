using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public CameraTouch cameraController;
    public GameObject map;
    public GameObject dotPrefab;
    public Sprite DEBUG_MAPSPRITE;
    private GameObject dot;
    // Start is called before the first frame update
    void Start()
    {
        PlaceMap(DEBUG_MAPSPRITE, -5, 5, -5, 5);
    }

    public void PlaceDot(float x, float y)
    {
        if (dot == null) dot = Instantiate(dotPrefab, transform);
        dot.transform.position = new Vector3(x, y, 2); //cos zjebane z tym 2d albo warstwami
    }

    public void RemoveDot()
    {
        Destroy(dot);
        dot = null;
    }

    void PlaceMap(Sprite map, float minX, float maxX, float minY, float maxY)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = map;
        cameraController.minX = minX;
        cameraController.maxX = maxX;
        cameraController.minY = minY;
        cameraController.maxY = maxY;

    }
}
