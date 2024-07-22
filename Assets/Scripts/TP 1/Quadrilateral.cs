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
}
