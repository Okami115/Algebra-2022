using System;
using System.Collections.Generic;
using UnityEngine;

public class RayForPlayer : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Frustrum frustrum;

    [SerializeField] private GameObject prefab;

    Camera camera;
    private float fov;

    [SerializeField] private Room currentRoom;
    [SerializeField] private Room[] Rooms;

    Vector3 centerNear;
    Vector3 centerFar;

    Vector3 colicion;

    Vector3 start;
    Vector3 end;

    Vector3 point;
    Vector3 dir;

    GameObject punto;
    GameObject corte;
    GameObject corte2;

    private List<Room> roomsChecked;

    private void Start()
    {
        roomsChecked = new List<Room>();

        camera = Camera.main;
        fov = camera.fieldOfView;

        punto = Instantiate(prefab);
        corte = Instantiate(prefab);
        corte2 = Instantiate(prefab);

        currentRoom.SetActiveRoom(true);
    }

    private void Update()
    {
        centerNear = (frustrum.nearPlane.a + frustrum.nearPlane.b + frustrum.nearPlane.c + frustrum.nearPlane.d) / 4.0f;
        centerFar = (frustrum.farPlane.a + frustrum.farPlane.b + frustrum.farPlane.c + frustrum.farPlane.d) / 4.0f;

        start = centerNear;
        end = centerFar;

        dir = start - end;

        point = (start + end / 2);

        CheckRooms();

        for (int i = 0; i < currentRoom.walls.Length; i++)
        {
            if (LineIntersectsPlane(end, dir.normalized, currentRoom.walls[i].center, currentRoom.walls[i].quadrilateral, currentRoom.walls[i].plane, out colicion))
            {
                if (Quadrilateral.IsPointInQuadrilateral(colicion, currentRoom.walls[i].quadrilateralDoor))
                {
                    punto.transform.position = colicion;
                    Debug.Log($"Pos in door : {colicion}");
                    break;
                }
                punto.transform.position = colicion;
                Debug.Log($"Pos : {colicion}");
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(start, colicion);
    }

    static bool LineIntersectsPlane(Vector3 linePoint, Vector3 lineDir, Vector3 planePoint, Quadrilateral quad, Plane plane, out Vector3 intersection)
    {
        intersection = new Vector3();

        float denominator = Vector3.Dot(plane.normal, lineDir);

        float distance = Vector3.Dot(plane.normal, linePoint) + (-Vector3.Dot(plane.normal, planePoint));

        if (distance > 0 || distance == 0)
        {
            return false;
        }

        if (Math.Abs(denominator) < 1e-6)
        {
            return false;
        }

        float t = Vector3.Dot(plane.normal, planePoint - linePoint) / denominator;

        intersection = linePoint + lineDir * t;


        if (!Quadrilateral.IsPointInQuadrilateral(intersection, quad))
        {
            return false;
        }

        return true;
    }

    private void CheckRooms()
    {
        bool inRoom = false;

        do
        {
            for (int i = 0; i < Rooms.Length; i++)
            {
                Plane[] walls = new Plane[4];

                for (int k = 0; k < Rooms[i].walls.Length; k++)
                {
                    walls[k] = Rooms[i].walls[k].plane;
                }

                if (AABBCalculator.IsInPlanes(point, walls))
                {
                    corte.transform.position = point;
                    inRoom = true;
                    Debug.Log($"IN ROOM : {i}");

                    if (Rooms[i] != currentRoom)
                    {
                        roomsChecked.Add(currentRoom);
                        SetRoom(Rooms[i], currentRoom.roomsAdyasent);
                    }

                    break;
                }
            }

            if (point == (start + point / 2))
                inRoom = true;

            if (!inRoom)
                point = (start + point / 2);

        }
        while (!inRoom);

        inRoom = false;
        point = (start + end / 2);

        do
        {
            for (int i = 0; i < Rooms.Length; i++)
            {
                Plane[] walls = new Plane[4];

                for (int k = 0; k < Rooms[i].walls.Length; k++)
                {
                    walls[k] = Rooms[i].walls[k].plane;
                }

                if (AABBCalculator.IsInPlanes(point, walls))
                {
                    corte2.transform.position = point;
                    inRoom = true;
                    Debug.Log($"IN ROOM : {i}");

                    if (Rooms[i] != currentRoom)
                    {
                        roomsChecked.Add(currentRoom);
                        SetRoom(Rooms[i], currentRoom.roomsAdyasent);
                    }

                    break;
                }
            }

            if (point == (point + end / 2))
                inRoom = true;

            if (!inRoom)
                point = (point + end / 2);

        }
        while (!inRoom);
    }

    private void SetRoom(Room room, Room[] adyasents)
    {
        for (int i = 0; i < adyasents.Length; i++)
        {
            if (room == adyasents[i])
            {
                room.SetActiveRoom(true);
                roomsChecked.Clear();
                break;
            }
            else
            {
                bool isChecked = false;

                for(int j = 0; j < roomsChecked.Count; j++)
                {
                    if (adyasents[i] == roomsChecked[j])
                    {
                        isChecked = true;
                    }
                    else
                    {
                        roomsChecked.Add(adyasents[i]);
                    }
                }

                if (!isChecked)
                    SetRoom(room, adyasents[i].roomsAdyasent);
            }

        }
    }
}
