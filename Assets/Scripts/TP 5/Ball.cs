using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    [SerializeField] private float fricction;

    public TypeBall typeBall;

    public float radius;
    public float mass;
    public Vector3 velocity;
    public Vector3 position;

    public bool isActive = true;

    private void Update()
    {
        position = transform.position;

        if (isActive)
        {
            for (int i = 0; i < balls.Length; i++)
            {
                if (balls[i].isActive && IsSphereCollidingWithSphere(position, radius, balls[i].position, balls[i].radius))
                {
                    CollisionWithSphere(balls[i]);
                }
            }

            if(Vector3.Distance(velocity, Vector3.zero) > Vector3.kEpsilon)
            {
                position.x += velocity.x * Time.deltaTime;
                position.y = 0.75f;
                position.z += velocity.z * Time.deltaTime;

                transform.position = position;

                Vector3 friction = -velocity.normalized * fricction * mass;

                if (friction.magnitude > velocity.magnitude / Time.deltaTime)
                {
                    friction = velocity * -1.0f / Time.deltaTime;
                }

                ApplyForce(friction);
            }
        }
        else
        {
            position = new Vector3(0, -100, 0);
        }
    }

    public static bool IsSphereCollidingWithSphere(Vector3 center1, float radius1, Vector3 center2, float radius2)
    {
        float distanceSquared = Vector3.SqrMagnitude(center1 - center2);

        float radiusSum = radius1 + radius2;

        return distanceSquared <= radiusSum * radiusSum;
    }

    public void ApplyForce(Vector3 force)
    {
        Vector3 acceleration = force / mass;

        velocity += acceleration * Time.deltaTime;

        position.x += velocity.x * Time.deltaTime;
        position.y = 0.75f;
        position.z += velocity.z * Time.deltaTime;

        transform.position = position;
    }

    private void CollisionWithSphere(Ball other)
    {
        // Calcula la normal del punto de colision
        Vector3 collisionNormal = (other.position - position).normalized;

        // Calcula la velocidad resultante de este choque
        Vector3 relativeVelocity = other.velocity - velocity;

        // orienta el vector de la velocidad hacia la normal de choque
        float velocityAlongNormal = Vector3.Dot(relativeVelocity, collisionNormal);

        if (velocityAlongNormal > 0)
            return;

        // Coeficiente de elasticidad
        float e = 1.0f;// Es completamente elastica

        //Impulso escalar que mantiene el momento del choque
        float j = -(1 + e) * velocityAlongNormal; // calcula la magnitud del impulso

        //Ajusta el impulso en funcion de la masa de cada bola
        j /= 1 / mass + 1 / other.mass;

        Vector3 impulse = j * collisionNormal;

        // Se aplica el impuso a cada bola dependiendo del lado del choque en el que esten
        velocity -= impulse / mass;
        other.velocity += impulse / other.mass;
    }
}

[Serializable]
public enum TypeBall
{
    White,
    Black,
    Smooth,
    Scratched
}
