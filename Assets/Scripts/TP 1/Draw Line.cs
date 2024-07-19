using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    Cuadrilatero cuadrilatero;

    Lateral[] setCuadrilatero;

    int iterator = 0;

    void Start()
    {
        setCuadrilatero = new Lateral[4];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setCuadrilatero[iterator].start = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }

        if (Input.GetMouseButtonUp(0))
        {
            setCuadrilatero[iterator].end = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            iterator++;

            if (iterator == setCuadrilatero.Length)
            {
                cuadrilatero = DoLinesIntersect(setCuadrilatero);
                iterator = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            setCuadrilatero = new Lateral[3];
            iterator = 0;
        }
    }

    Cuadrilatero DoLinesIntersect(Lateral[] laterales)
    {
        // Teoria de la interseccion de 2 segmentos
        //"https://es.wikipedia.org/wiki/Intersecci%C3%B3n_(geometr%C3%ADa)"

        List<Vector3> points = new List<Vector3>();

        float denominator = (laterales[0].start.x - laterales[0].end.x) * (laterales[1].start.y - laterales[1].end.y) - (laterales[0].start.y - laterales[0].end.y) * (laterales[1].start.x - laterales[1].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[0].start.x - laterales[1].start.x) * (laterales[1].start.y - laterales[1].end.y) - (laterales[0].start.y - laterales[1].start.y) * (laterales[1].start.x - laterales[1].end.x)) / denominator;
            float u = ((laterales[0].start.x - laterales[1].start.x) * (laterales[0].start.y - laterales[0].end.y) - (laterales[0].start.y - laterales[1].start.y) * (laterales[0].start.x - laterales[0].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[0].start.x + t * (laterales[0].end.x - laterales[0].start.x), laterales[0].start.y + t * (laterales[0].end.y - laterales[0].start.y), -9.7f));
            }
        }

        denominator = (laterales[0].start.x - laterales[0].end.x) * (laterales[2].start.y - laterales[2].end.y) - (laterales[0].start.y - laterales[0].end.y) * (laterales[2].start.x - laterales[2].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[0].start.x - laterales[2].start.x) * (laterales[2].start.y - laterales[2].end.y) - (laterales[0].start.y - laterales[2].start.y) * (laterales[2].start.x - laterales[2].end.x)) / denominator;
            float u = ((laterales[0].start.x - laterales[2].start.x) * (laterales[0].start.y - laterales[0].end.y) - (laterales[0].start.y - laterales[2].start.y) * (laterales[0].start.x - laterales[0].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[0].start.x + t * (laterales[0].end.x - laterales[0].start.x), laterales[0].start.y + t * (laterales[0].end.y - laterales[0].start.y), -9.7f));
            }
        }

        denominator = (laterales[0].start.x - laterales[0].end.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[0].start.y - laterales[0].end.y) * (laterales[3].start.x - laterales[3].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[0].start.x - laterales[3].start.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[0].start.y - laterales[3].start.y) * (laterales[3].start.x - laterales[3].end.x)) / denominator;
            float u = ((laterales[0].start.x - laterales[3].start.x) * (laterales[0].start.y - laterales[0].end.y) - (laterales[0].start.y - laterales[3].start.y) * (laterales[0].start.x - laterales[0].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[0].start.x + t * (laterales[0].end.x - laterales[0].start.x), laterales[0].start.y + t * (laterales[0].end.y - laterales[0].start.y), -9.7f));
            }
        }

        denominator = (laterales[1].start.x - laterales[1].end.x) * (laterales[2].start.y - laterales[2].end.y) - (laterales[1].start.y - laterales[1].end.y) * (laterales[2].start.x - laterales[2].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[1].start.x - laterales[2].start.x) * (laterales[2].start.y - laterales[2].end.y) - (laterales[1].start.y - laterales[2].start.y) * (laterales[2].start.x - laterales[2].end.x)) / denominator;
            float u = ((laterales[1].start.x - laterales[2].start.x) * (laterales[1].start.y - laterales[1].end.y) - (laterales[1].start.y - laterales[2].start.y) * (laterales[1].start.x - laterales[1].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[1].start.x + t * (laterales[1].end.x - laterales[1].start.x), laterales[1].start.y + t * (laterales[1].end.y - laterales[1].start.y), -9.7f));
            }
        }

        denominator = (laterales[1].start.x - laterales[1].end.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[1].start.y - laterales[1].end.y) * (laterales[3].start.x - laterales[3].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[1].start.x - laterales[3].start.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[1].start.y - laterales[3].start.y) * (laterales[3].start.x - laterales[3].end.x)) / denominator;
            float u = ((laterales[1].start.x - laterales[3].start.x) * (laterales[1].start.y - laterales[1].end.y) - (laterales[1].start.y - laterales[3].start.y) * (laterales[1].start.x - laterales[1].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[1].start.x + t * (laterales[1].end.x - laterales[1].start.x), laterales[1].start.y + t * (laterales[1].end.y - laterales[1].start.y), -9.7f));
            }
        }

        denominator = (laterales[2].start.x - laterales[2].end.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[2].start.y - laterales[2].end.y) * (laterales[3].start.x - laterales[3].end.x);

        if (denominator != 0)
        {
            float t = ((laterales[2].start.x - laterales[3].start.x) * (laterales[3].start.y - laterales[3].end.y) - (laterales[2].start.y - laterales[3].start.y) * (laterales[3].start.x - laterales[3].end.x)) / denominator;
            float u = ((laterales[2].start.x - laterales[3].start.x) * (laterales[2].start.y - laterales[2].end.y) - (laterales[2].start.y - laterales[3].start.y) * (laterales[2].start.x - laterales[2].end.x)) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                points.Add(new Vector3(laterales[2].start.x + t * (laterales[2].end.x - laterales[2].start.x), laterales[2].start.y + t * (laterales[2].end.y - laterales[2].start.y), -9.7f));
            }
        }

        if(points.Count == 4)
        {
            return new Cuadrilatero(points[0], points[1], points[2], points[3]);
        }
        else
        {
            return new Cuadrilatero();
        }

    }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
            return;


        Gizmos.color = Color.yellow;

        for (int i = 0; i < setCuadrilatero.Length; i++)
        {
            Gizmos.DrawLine(setCuadrilatero[i].start, setCuadrilatero[i].end);
        }



        Gizmos.color = Color.white;
        Gizmos.DrawLine(cuadrilatero.a, cuadrilatero.b);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cuadrilatero.b, cuadrilatero.d);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cuadrilatero.c, cuadrilatero.d);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(cuadrilatero.c, cuadrilatero.a);
    }
}

public struct Cuadrilatero
{
    public Vector3 a, b, c, d;

    public Cuadrilatero(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        this.a = a; 
        this.b = b; 
        this.c = c;
        this.d = d;
    }
}

public struct Lateral
{
    public Vector3 start;
    public Vector3 end;

    public Lateral(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end; 
    }
}
