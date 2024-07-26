using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isVisble;
    [SerializeField] private Vector3[] door;
    [SerializeField] private int[] vertex;

    private Plane plane;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private Vector3[] vertices;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        vertex = new int[4] { 0, 1, 2, 3 };

        mesh = meshFilter.mesh;

        vertices = mesh.vertices;

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);
        }
    }

    private void Update()
    {
        plane = new Plane(vertices[vertex[0]], vertices[vertex[1]], vertices[vertex[2]]);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(vertices[vertex[0]], vertices[vertex[1]]);
        Gizmos.DrawLine(vertices[vertex[1]], vertices[vertex[2]]);
        Gizmos.DrawLine(vertices[vertex[2]], vertices[vertex[3]]);
        Gizmos.DrawLine(vertices[vertex[3]], vertices[vertex[0]]);

        Gizmos.color = Color.magenta;

        Vector3 center = (vertices[vertex[0]] + vertices[vertex[1]] + vertices[vertex[2]] + vertices[vertex[3]]) / 4.0f;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(center, center + plane.normal);
    }
}
