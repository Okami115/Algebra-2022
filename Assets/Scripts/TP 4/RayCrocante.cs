using System;
using System.Collections.Generic;
using UnityEngine;

public class RayCrocante : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Frustrum frustrum;

    [SerializeField] private Room[] Rooms;
    private Room currentRoom;

    [SerializeField] private GameObject prefab;

    Vector3 start;
    Vector3 end;
    Vector3 dir;
    Vector3 colision;

    GameObject punto;

    private List<Room> roomsChecked;

    private void Start()
    {
        roomsChecked = new List<Room>();
        punto = Instantiate(prefab);
    }

    private void Update()
    {
        start = (frustrum.nearPlane.a + frustrum.nearPlane.b + frustrum.nearPlane.c + frustrum.nearPlane.d) / 4.0f;
        end = (frustrum.farPlane.a + frustrum.farPlane.b + frustrum.farPlane.c + frustrum.farPlane.d) / 4.0f;

        dir = start - end;

        CheckCurrentRoom();

        colision = CheckRay(currentRoom);

        punto.transform.position = colision;
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

    private void CheckCurrentRoom()
    {
        for (int i = 0; i < Rooms.Length; i++)
        {
            Plane[] walls = new Plane[4];

            for (int k = 0; k < Rooms[i].walls.Length; k++)
            {
                walls[k] = Rooms[i].walls[k].plane;
            }

            if (AABBCalculator.IsInPlanes(transform.position, walls))
            {
                Rooms[i].SetActiveRoom(true);
                currentRoom = Rooms[i];
            }
            else
            {
                Rooms[i].SetActiveRoom(false);
            }
        }
    }

    private Vector3 CheckRay(Room room)
    {
        Vector3 outVec3 = Vector3.zero;

        for (int i = 0; i < room.walls.Length; i++)
        {
            if (LineIntersectsPlane(end, dir.normalized, room.walls[i].center, room.walls[i].quadrilateral, room.walls[i].plane, out outVec3))
            {
                if (Quadrilateral.IsPointInQuadrilateral(outVec3, room.walls[i].quadrilateralDoor))
                {
                    for (int j = 0; j < Rooms.Length; j++)
                    {
                        if (!roomsChecked.Contains(Rooms[j]))
                        {
                            roomsChecked.Add(Rooms[j]);
                            Vector3 aux = CheckRay(Rooms[j]);

                            if (aux != Vector3.zero)
                            {
                                Debug.Log($"Pos in door in {room.gameObject.name} : {outVec3}");
                                room.SetActiveRoom(true);
                                roomsChecked.Clear();
                                return aux;
                            }
                        }
                    }

                    if (roomsChecked.Count == Rooms.Length)
                    {
                        Debug.Log($"Frustrum: {outVec3}");
                        roomsChecked.Clear();
                        return end;
                    }
                }

                Debug.Log($"Pos in {room.gameObject.name}: {outVec3}");
                room.SetActiveRoom(true);
                return outVec3;
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(start, colision);
    }
}
