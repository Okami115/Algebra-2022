using Unity.VisualScripting;
using UnityEngine;

public struct Quadrilateral
{
    public Vector3 a, b, c, d;

    public Quadrilateral(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        this.a = a; 
        this.b = b; 
        this.c = c;
        this.d = d;
    }

    static public bool IsPointInQuadrilateral(Vector3 point, Quadrilateral quad)
    {
        int intersections = 0;

        if (IsIntersecting(point, quad.a, quad.b, quad))
        {
            intersections++;
        }

        if (IsIntersecting(point, quad.b, quad.c, quad))
        {
            intersections++;
        }

        if (IsIntersecting(point, quad.c, quad.d, quad))
        {
            intersections++;
        }

        if (IsIntersecting(point, quad.d, quad.a, quad))
        {
            intersections++;
        }

        return (intersections % 2) == 1;
    }

    static bool IsIntersecting(Vector3 point, Vector3 v1, Vector3 v2, Quadrilateral quad)
    {
        if(quad.a.z == quad.b.z && quad.b.z == quad.c.z && quad.c.z == quad.d.z)
        {
            if (v1.y > v2.y)
            {
                Vector3 temp = v1;
                v1 = v2;
                v2 = temp;
            }

            if (point.y == v1.y || point.y == v2.y)
            {
                point.y += 0.0001f;
            }

            if (point.y < v1.y || point.y > v2.y)
            {
                return false;
            }

            if (point.x > Mathf.Max(v1.x, v2.x))
            {
                return false;
            }

            if (point.x < Mathf.Min(v1.x, v2.x))
            {
                return true;
            }

            float xinters = (point.y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x;

            return xinters > point.x;
        }
        else
        {

            if (v1.y > v2.y)
            {
                Vector3 temp = v1;
                v1 = v2;
                v2 = temp;
            }

            if (point.y == v1.y || point.y == v2.y)
            {
                point.y += 0.0001f;
            }

            if (point.y < v1.y || point.y > v2.y)
            {
                return false;
            }

            if (point.z > Mathf.Max(v1.z, v2.z))
            {
                return false;
            }

            if (point.z < Mathf.Min(v1.z, v2.z))
            {
                return true;
            }

            float xinters = (point.y - v1.y) * (v2.z - v1.z) / (v2.y - v1.y) + v1.z;

            return xinters > point.z;
        }
    }
}
