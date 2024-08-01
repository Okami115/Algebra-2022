using System;
using UnityEngine;

public class BoxColider : MonoBehaviour
{
    [SerializeField] private Ball[] balls;

    [SerializeField] private MeshFilter meshFilter;

    Vector3 aabbMin;
    Vector3 aabbMax;

    private void Start()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        aabbMin = transform.TransformPoint(vertices[0]);
        aabbMax = aabbMin;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = transform.TransformPoint(vertex);

            aabbMin = Vector3.Min(aabbMin, worldVertex);
            aabbMax = Vector3.Max(aabbMax, worldVertex);
        }
    }

    private void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            if (IsSphereCollidingWithCube(balls[i].position, balls[i].radius, aabbMin, aabbMax))
            {
                Vector3 closestPoint = new Vector3(
                    Mathf.Max(aabbMin.x, Mathf.Min(balls[i].position.x, aabbMax.x)),
                    Mathf.Max(aabbMin.y, Mathf.Min(balls[i].position.y, aabbMax.y)),
                    Mathf.Max(aabbMin.z, Mathf.Min(balls[i].position.z, aabbMax.z)));

                ReflectFromCube(balls[i], closestPoint);
            }
        }
    }

    public static bool IsSphereCollidingWithCube(Vector3 sphereCenter, float sphereRadius, Vector3 cubeMin, Vector3 cubeMax)
    {
        Vector3 closestPoint = new Vector3(
            Math.Max(cubeMin.x, Math.Min(sphereCenter.x, cubeMax.x)),
            Math.Max(cubeMin.y, Math.Min(sphereCenter.y, cubeMax.y)),
            Math.Max(cubeMin.z, Math.Min(sphereCenter.z, cubeMax.z))
        );

        Vector3 aux = closestPoint - sphereCenter;

        float distanceSquared = Vector3.SqrMagnitude(aux);

        return distanceSquared <= sphereRadius * sphereRadius;
    }

    public void ReflectFromCube(Ball ball, Vector3 closestPoint)
    {
        Vector3 normal = new Vector3();

        if (ball.position.x < aabbMin.x) normal = new Vector3(-1, 0, 0);
        else if (ball.position.x > aabbMax.x) normal = new Vector3(1, 0, 0);
        else if (ball.position.y < aabbMin.y) normal = new Vector3(0, -1, 0);
        else if (ball.position.y > aabbMax.y) normal = new Vector3(0, 1, 0);
        else if (ball.position.z < aabbMin.z) normal = new Vector3(0, 0, -1);
        else if (ball.position.z > aabbMax.z) normal = new Vector3(0, 0, 1);

        ball.gameObject.transform.position = closestPoint + normal * ball.radius;

        ball.velocity = Vector3.Reflect(ball.velocity, normal);
    }
}
