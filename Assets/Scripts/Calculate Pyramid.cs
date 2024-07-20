using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CalculatePyramid : MonoBehaviour
{

    [SerializeField] GameObject prefab;

    Side vec1;
    Side vec2;
    Side vec3;

    Vector3 a, b, c;

    GameObject[] points = new GameObject[3];


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Destroy(points[0]);
            Destroy(points[1]);
            Destroy(points[2]);

            vec1 = new Side(Vector3.zero, GetRandomVector());
            vec2 = new Side(Vector3.zero, new Vector3(-vec1.end.z, vec1.end.x, vec1.end.y));
            vec3 = new Side(Vector3.zero, GetCrossProduct(vec1.end, vec2.end));

            Debug.Log($"Vector 1 : {vec1.end.y}");
            Debug.Log($"Vector 2 : {vec2.end.y}");
            Debug.Log($"Vector 3 : {vec3.end.y}");

            if (vec1.end.y < vec2.end.y && vec1.end.y < vec3.end.y || vec1.end.y == vec2.end.y || vec1.end.y == vec3.end.y)
            {
                a = vec1.end;

                b.y = a.y;
                b.x = (vec2.end.x / vec2.end.y) * b.y;
                b.z = (vec2.end.z / vec2.end.y) * b.y;

                c.y = a.y;
                c.x = (vec3.end.x / vec3.end.y) * b.y;
                c.z = (vec3.end.z / vec3.end.y) * b.y;
            }
            else if (vec2.end.y < vec1.end.y && vec2.end.y < vec3.end.y || vec3.end.y == vec2.end.y)
            {
                a = vec2.end;

                b.y = a.y;
                b.x = (vec1.end.x / vec1.end.y) * b.y;
                b.z = (vec1.end.z / vec1.end.y) * b.y;

                c.y = a.y;
                c.x = (vec3.end.x / vec3.end.y) * b.y;
                c.z = (vec3.end.z / vec3.end.y) * b.y;
            }
            else
            {
                a = vec3.end;

                b.y = a.y;
                b.x = (vec1.end.x / vec1.end.y) * b.y;
                b.z = (vec1.end.z / vec1.end.y) * b.y;

                c.y = a.y;
                c.x = (vec2.end.x / vec2.end.y) * b.y;
                c.z = (vec2.end.z / vec2.end.y) * b.y;
            }

            points[0] = Instantiate(prefab, a, Quaternion.identity);
            points[1] = Instantiate(prefab, b, Quaternion.identity);
            points[2] = Instantiate(prefab, c, Quaternion.identity);

            Debug.Log($"Area : {GetTraingleArea(a, b, c)}");

            float surfaceArea1 = GetTraingleArea(a, b, Vector3.zero);
            float surfaceArea2 = GetTraingleArea(b, c, Vector3.zero);
            float surfaceArea3 = GetTraingleArea(c, a, Vector3.zero);

            float totalSurfaceArea = surfaceArea1 + surfaceArea2 + surfaceArea3;

            Debug.Log($"Total Surface Area: {totalSurfaceArea}");

        }
    }
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(vec1.start, vec1.end);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(vec2.start, vec2.end);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(vec3.start, vec3.end);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, a);
    }


    Vector3 GetRandomVector()
    {
        Vector3 outvec = new Vector3();

        outvec.x = Random.Range(1, 5);
        outvec.y = Random.Range(1, 5);
        outvec.z = Random.Range(1, 5);

        return outvec;
    }

    Vector3 GetCrossProduct(Vector3 vecA, Vector3 vecB)
    {
        Vector3 outvec = new Vector3();

        outvec.x = ((vecA.y * vecB.z) - (vecA.z * vecB.y));
        outvec.z = ((vecA.z * vecB.x) - (vecA.x * vecB.z));
        outvec.y = ((vecA.x * vecB.y) - (vecA.y * vecB.x));

        return outvec;
    }

    float GetTraingleArea(Vector3 a, Vector3 b, Vector3 c)
    {
        float side1 = Vector3.Distance(a, b);
        float side2 = Vector3.Distance(b, c);
        float side3 = Vector3.Distance(c, a);

        float s = (side1 + side2 + side3) / 2;

        return Mathf.Sqrt(s * (s - side1) * (s - side2) * (s - side3));
    }
}
