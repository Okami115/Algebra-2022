using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] private Ball ball;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ball.ApplyForce(new Vector3(100, 0, 0));
        }
    }
}
