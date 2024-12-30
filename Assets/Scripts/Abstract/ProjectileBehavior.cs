using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    protected float projectileSpeed = 1f;
    protected float dropOffDistance = 1f;
    protected float dropOffSpeed = 1f;
    protected float lifeSpan = 10f;
    public int Damage { get; protected set; }

    //Trajectory values
    private Vector3 velocity;
    private bool dropOff = false;
    private Vector3 startPosition;

    //Gameobjects and components
    private Rigidbody rb;
    private GameObject explosion;

    private bool collided = false;

    protected virtual void Awake()
    {

    }


    protected virtual void Update()
    {
        Trajectory();

        if (!dropOff)
        {
            CalculateWhenDropOff();
        }
        else
        {
            RotateDownwards();
        }

        rb.velocity = velocity;

        lifeSpan = lifeSpan - Time.deltaTime;

        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * projectileSpeed;
    }

    private void RotateDownwards()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + dropOffSpeed, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void CalculateWhenDropOff()
    {
        Vector3 currentPostion = transform.position;
        float distance = Vector3.Distance(startPosition, currentPostion);

        if (distance > dropOffDistance)
        {
            dropOff = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            collided = true; //needed that no double collissios lead to double explosions because of rockets bigger models / will maybe be sitched to the missle script but could be useful for other projectiles too
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Instantiate(explosion, position, rotation);
            Destroy(gameObject);
        }
    }
}
