using Unity.VisualScripting;
using UnityEngine;

public class Frustrum : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public Plane[] planes = new Plane[6];

    Quadrilateral nearPlane;
    Quadrilateral farPlane;
    Quadrilateral topPlane;
    Quadrilateral bottonPlane;
    Quadrilateral leftPlane;
    Quadrilateral rightPlane;

    void Start()
    {
        UpdateFrustrum();
    }

    void Update()
    {
        UpdateFrustrum();
    }

    void UpdateFrustrum()
    {
        float fovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;

        float nearHeight = 2.0f * Mathf.Tan(fovRad / 2.0f) * Camera.main.nearClipPlane;
        float nearWidth = nearHeight * Camera.main.aspect;

        float farHeight = 2.0f * Mathf.Tan(fovRad / 2.0f) * Camera.main.farClipPlane;
        float farWidth = farHeight * Camera.main.aspect;

        Vector3 centerNear = transform.position + transform.forward * Camera.main.nearClipPlane;
        Vector3 centerFar = transform.position + transform.forward * Camera.main.farClipPlane;

        Vector3 nearTopLeft = centerNear + (transform.up * nearHeight / 2.0f) - (transform.right * nearWidth / 2.0f);
        Vector3 nearTopRight = centerNear + (transform.up * nearHeight / 2.0f) + (transform.right * nearWidth / 2.0f);
        Vector3 nearBottomLeft = centerNear - (transform.up * nearHeight / 2.0f) - (transform.right * nearWidth / 2.0f);
        Vector3 nearBottomRight = centerNear - (transform.up * nearHeight / 2.0f) + (transform.right * nearWidth / 2.0f);

        Vector3 farTopLeft = centerFar + (transform.up * farHeight / 2.0f) - (transform.right * farWidth / 2.0f);
        Vector3 farTopRight = centerFar + (transform.up * farHeight / 2.0f) + (transform.right * farWidth / 2.0f);
        Vector3 farBottomLeft = centerFar - (transform.up * farHeight / 2.0f) - (transform.right * farWidth / 2.0f);
        Vector3 farBottomRight = centerFar - (transform.up * farHeight / 2.0f) + (transform.right * farWidth / 2.0f);

        planes[0] = new Plane(nearTopLeft, nearBottomLeft, nearBottomRight);
        nearPlane = new Quadrilateral(nearTopLeft, nearBottomLeft, nearBottomRight, nearTopRight);

        planes[1] = new Plane(farTopRight, farBottomRight, farBottomLeft);
        farPlane = new Quadrilateral(farTopLeft, farBottomRight, farBottomLeft, farTopRight);

        planes[2] = new Plane(nearTopLeft, farTopLeft, farBottomLeft);
        leftPlane = new Quadrilateral(nearTopLeft, farTopLeft, farBottomLeft, nearBottomLeft);

        planes[3] = new Plane(farTopRight, nearTopRight, farBottomRight);
        rightPlane = new Quadrilateral(farTopRight, nearTopRight, farBottomRight, nearBottomRight);

        planes[4] = new Plane(nearTopRight, farTopRight, farTopLeft);
        topPlane = new Quadrilateral(nearTopRight, farTopRight, farTopLeft, nearTopLeft);

        planes[5] = new Plane(nearBottomLeft, farBottomLeft, farBottomRight);
        bottonPlane = new Quadrilateral(nearBottomLeft, farBottomLeft, farBottomRight, nearBottomRight);
    }

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        DrawFrustrum();

        DrawPlaneNormal(nearPlane, planes[0]);
        DrawPlaneNormal(farPlane, planes[1]);
        DrawPlaneNormal(leftPlane, planes[2]);
        DrawPlaneNormal(rightPlane, planes[3]);
        DrawPlaneNormal(topPlane, planes[4]);
        DrawPlaneNormal(bottonPlane, planes[5]);
    }

    void DrawPlaneNormal(Quadrilateral planes, Plane plane)
    {
        Vector3 center = (planes.a + planes.b + planes.c + planes.d) / 4.0f;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(center, center + plane.normal);
    }

    void DrawFrustrum()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(farPlane.c, farPlane.b);
        Gizmos.DrawLine(farPlane.a, farPlane.c);
        Gizmos.DrawLine(farPlane.b, farPlane.d);
        Gizmos.DrawLine(farPlane.d, farPlane.a);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(nearPlane.a, nearPlane.b);
        Gizmos.DrawLine(nearPlane.b, nearPlane.c);
        Gizmos.DrawLine(nearPlane.c, nearPlane.d);
        Gizmos.DrawLine(nearPlane.d, nearPlane.a);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(nearPlane.a, farPlane.a);
        Gizmos.DrawLine(nearPlane.c, farPlane.b);
        Gizmos.DrawLine(nearPlane.b, farPlane.c);
        Gizmos.DrawLine(nearPlane.d, farPlane.d);

    }
}
