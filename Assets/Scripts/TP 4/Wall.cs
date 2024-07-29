using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Wall")]
    [SerializeField] private int[] vertex;

    [Header("Holes")]
    [SerializeField] private bool isVisble;
    [SerializeField] private Vector3[] vertexDoor;

    private Plane plane;
    private Plane doorPlane;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private Vector3[] vertices;
    private Vector3[] verticesDoor;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.mesh;

        vertices = mesh.vertices;

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);
        }

        if(isVisble)
        {
            verticesDoor = new Vector3[4];

            for (int i = 0; i < vertexDoor.Length; i++)
            {
                verticesDoor[i] = transform.TransformPoint(vertexDoor[i]);
            }
        }
    }

    private void Update()
    {
        plane = new Plane(vertices[vertex[0]], vertices[vertex[1]], vertices[vertex[2]]);

        if(isVisble)
        {
            doorPlane = new Plane(verticesDoor[0], verticesDoor[1], verticesDoor[2]);
        }
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

        Vector3 center = (vertices[vertex[0]] + vertices[vertex[1]] + vertices[vertex[2]] + vertices[vertex[3]]) / 4.0f;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(center, center + plane.normal);

        if(isVisble)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(verticesDoor[0], verticesDoor[1]);
            Gizmos.DrawLine(verticesDoor[1], verticesDoor[2]);
            Gizmos.DrawLine(verticesDoor[2], verticesDoor[3]);
            Gizmos.DrawLine(verticesDoor[3], verticesDoor[0]);

            Vector3 doorCenter = (verticesDoor[0] + verticesDoor[1] + verticesDoor[2] + verticesDoor[3]) / 4.0f;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(doorCenter, doorCenter + doorPlane.normal);
        }
    }
}
