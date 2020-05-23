using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTouch : MonoBehaviour
{
    public GameObject map;
    public Camera camera;

    public float maxX;
    public float minX;

    public float maxY;
    public float minY;

    public float maxZoom;
    public float minZoom;

    float CameraZoom;
    Vector2 CameraPosition;
    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;


    // Update is called once per frame
    void Update()
    {
        
        CameraPosition = camera.transform.position;
        CameraZoom = camera.orthographicSize;

        if (Input.touchCount == 0 && isZooming)
        {
            isZooming = false;
        }

        if (Input.touchCount == 1)
        {
            if (!isZooming)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 NewPosition = GetWorldPosition();
                    Vector2 PositionDifference = NewPosition - StartPosition;
                    camera.transform.Translate(-PositionDifference);
                }
                StartPosition = GetWorldPosition();
            }
        }
        else if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                isZooming = true;

                DragNewPosition = GetWorldPositionOfFinger(1);
                Vector2 PositionDifference = DragNewPosition - DragStartPosition;

                if (Vector2.Distance(DragNewPosition, Finger0Position) < DistanceBetweenFingers) //przyblizanie
                    CameraZoom += (PositionDifference.magnitude);

                if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers) //oddalanie
                    CameraZoom -= (PositionDifference.magnitude);

                DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
            }
            DragStartPosition = GetWorldPositionOfFinger(1);
            Finger0Position = GetWorldPositionOfFinger(0);
        }
        CameraZoom = Mathf.Clamp(CameraZoom, minZoom, maxZoom);
        CameraPosition.x = Mathf.Clamp(camera.transform.position.x, minX, maxX);
        CameraPosition.y = Mathf.Clamp(camera.transform.position.y, minY, maxY);
        transform.position = CameraPosition;
        camera.orthographicSize = CameraZoom;


    }

    Vector2 GetWorldPosition()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetWorldPositionOfFinger(int FingerIndex)
    {
        return camera.ScreenToWorldPoint(Input.GetTouch(FingerIndex).position);
    }
}