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
        PlaceDot(2, 1); // tutaj dla kazdej mapy z db jakies koordynaty gdzie postawic kropke?
        PlaceMap(DEBUG_MAPSPRITE, -5, 5, -5, 5); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaceDot(float x, float y)
    {
        dot = Instantiate(dotPrefab);
        dot.transform.position = new Vector3(x, y, 2); //cos zjebane z tym 2d albo warstwami
    }

    void RemoveDot()
    {
        Destroy(dot);
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
