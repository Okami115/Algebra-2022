using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    [SerializeField] private float fricction;

    public float radius;
    public float mass;
    public Vector3 velocity;
    public Vector3 position;

    private void Update()
    {
        position = transform.position;

        for (int i = 0; i < balls.Length; i++)
        {
            if (IsSphereCollidingWithSphere(position, radius, balls[i].position, balls[i].radius))
            {
                CollisionWithSphere(balls[i]);
            }
        }

        if(velocity != Vector3.zero)
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
        Vector3 collisionNormal = (other.position - position).normalized;
        Vector3 relativeVelocity = other.velocity - velocity;

        float velocityAlongNormal = Vector3.Dot(relativeVelocity, collisionNormal);

        if (velocityAlongNormal > 0)
            return;

        // Coeficiente de elasticidad
        float e = 1.0f;// Es completamente eslastica

        float j = -(1 + e) * velocityAlongNormal;
        j /= 1 / mass + 1 / other.mass;

        Vector3 impulse = j * collisionNormal;
        velocity -= impulse / mass;
        other.velocity += impulse / other.mass;
    }
}
