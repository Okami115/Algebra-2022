using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Wall")]
    [SerializeField] private int[] vertex;

    [Header("Holes")]
    [SerializeField] public bool isVisble;
    [SerializeField] private Vector3[] vertexDoor;

    public Plane plane;
    public Plane doorPlane;
    private Mesh mesh;
    private MeshFilter meshFilter;

    public Quadrilateral quadrilateral;
    public Quadrilateral quadrilateralDoor;

    public Vector3 center;
    public Vector3 doorCenter;

    public Vector3[] vertices;
    public Vector3[] verticesDoor;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.mesh;

        vertices = mesh.vertices;

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);
        }

        plane = new Plane(vertices[vertex[0]], vertices[vertex[1]], vertices[vertex[2]]);


        if (isVisble)
        {
            verticesDoor = new Vector3[4];

            for (int i = 0; i < vertexDoor.Length; i++)
            {
                verticesDoor[i] = transform.TransformPoint(vertexDoor[i]);
            }

            doorPlane = new Plane(verticesDoor[0], verticesDoor[1], verticesDoor[2]);
            quadrilateralDoor = new Quadrilateral(verticesDoor[0], verticesDoor[1], verticesDoor[2], verticesDoor[3]);
        }

        quadrilateral = new Quadrilateral(vertices[vertex[0]], vertices[vertex[1]], vertices[vertex[2]], vertices[vertex[3]]);

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

        center = (vertices[vertex[0]] + vertices[vertex[1]] + vertices[vertex[2]] + vertices[vertex[3]]) / 4.0f;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(center, center + plane.normal);

        if(isVisble)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(verticesDoor[0], verticesDoor[1]);
            Gizmos.DrawLine(verticesDoor[1], verticesDoor[2]);
            Gizmos.DrawLine(verticesDoor[2], verticesDoor[3]);
            Gizmos.DrawLine(verticesDoor[3], verticesDoor[0]);

            doorCenter = (verticesDoor[0] + verticesDoor[1] + verticesDoor[2] + verticesDoor[3]) / 4.0f;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(doorCenter, doorCenter + doorPlane.normal);
        }
    }
}
