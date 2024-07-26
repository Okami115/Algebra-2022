using UnityEngine;

public class AABBCalculator : MonoBehaviour
{
    private Vector3 aabbMin;
    private Vector3 aabbMax;

    [SerializeField] Frustrum frustrum;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    private void Start()
    {
        frustrum = FindAnyObjectByType<Frustrum>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = meshFilter.mesh;
    }

    void Update()
    {
        CalculateAABB();
    }

    void CalculateAABB()
    {
        Vector3[] vertices = mesh.vertices;

        aabbMin = transform.TransformPoint(vertices[0]);
        aabbMax = aabbMin;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = transform.TransformPoint(vertex);

            aabbMin = Vector3.Min(aabbMin, worldVertex);
            aabbMax = Vector3.Max(aabbMax, worldVertex);
        }

        meshRenderer.enabled = IsAABBCollidingWithFrustum(aabbMin, aabbMax);
    }

    bool IsAABBCollidingWithFrustum(Vector3 aabbMin, Vector3 aabbMax)
    {
        foreach (Plane plane in frustrum.planes)
        {
            if (plane.GetDistanceToPoint(new Vector3(aabbMin.x, aabbMin.y, aabbMin.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMax.x, aabbMin.y, aabbMin.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMin.x, aabbMax.y, aabbMin.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMax.x, aabbMax.y, aabbMin.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMin.x, aabbMin.y, aabbMax.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMax.x, aabbMin.y, aabbMax.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMin.x, aabbMax.y, aabbMax.z)) > 0) continue;
            if (plane.GetDistanceToPoint(new Vector3(aabbMax.x, aabbMax.y, aabbMax.z)) > 0) continue;

            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (aabbMin != Vector3.zero && aabbMax != Vector3.zero)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMin.y, aabbMin.z), new Vector3(aabbMax.x, aabbMin.y, aabbMin.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMin.y, aabbMin.z), new Vector3(aabbMax.x, aabbMax.y, aabbMin.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMax.y, aabbMin.z), new Vector3(aabbMin.x, aabbMax.y, aabbMin.z));
            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMax.y, aabbMin.z), new Vector3(aabbMin.x, aabbMin.y, aabbMin.z));

            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMin.y, aabbMax.z), new Vector3(aabbMax.x, aabbMin.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMin.y, aabbMax.z), new Vector3(aabbMax.x, aabbMax.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMax.y, aabbMax.z), new Vector3(aabbMin.x, aabbMax.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMax.y, aabbMax.z), new Vector3(aabbMin.x, aabbMin.y, aabbMax.z));

            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMin.y, aabbMin.z), new Vector3(aabbMin.x, aabbMin.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMin.y, aabbMin.z), new Vector3(aabbMax.x, aabbMin.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMax.x, aabbMax.y, aabbMin.z), new Vector3(aabbMax.x, aabbMax.y, aabbMax.z));
            Gizmos.DrawLine(new Vector3(aabbMin.x, aabbMax.y, aabbMin.z), new Vector3(aabbMin.x, aabbMax.y, aabbMax.z));
        }
    }
}
