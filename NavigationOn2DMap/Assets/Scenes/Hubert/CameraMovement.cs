using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public GameObject map;
    public Camera camera;
    public Text debugtext;
    public Text camPos;
    public Text camSize;
    public float minX;
    public float minY;

    public float maxX;
    public float maxY;

    Vector2 CameraPosition;
    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;

    Vector2 ViewPoint;

    // Update is called once per frame
    void Update()
    {
        camPos.text = camera.transform.position.ToString();
        camSize.text = "size:"+camera.orthographicSize.ToString();


        CameraPosition = camera.transform.position;

        ViewPoint = camera.WorldToViewportPoint(map.transform.position);

        if (ViewPoint.x > 0 || ViewPoint.x < 1 || ViewPoint.y > 0 || ViewPoint.y < 1)
        {
            //camera cant see object
            debugtext.text = "MapVisible!";
        }
        else
        {
            debugtext.text = "Cant see map!";
        }


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
                    camera.orthographicSize += (PositionDifference.magnitude);

                if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers) //oddalanie
                    camera.orthographicSize -= (PositionDifference.magnitude);

                DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
            }
            DragStartPosition = GetWorldPositionOfFinger(1);
            Finger0Position = GetWorldPositionOfFinger(0);
        }

        CameraPosition.x = Mathf.Clamp(camera.transform.position.x, minX, maxX);
        CameraPosition.y = Mathf.Clamp(camera.transform.position.y, minY, maxY);
        transform.position = CameraPosition;


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