using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissleBehavior : MonoBehaviour
{
    //Missle values
    private float missleSpeed = 7f;
    private float dropOffDistance = 20f;
    private float dropOffSpeed = 0.06f;

    //Trajectory values
    private Vector3 velocity;
    private bool dropOff = false;
    private Vector3 startPosition;

    //Gameobjects and components
    private Rigidbody rb;
    private GameObject explosion;

    private bool collided = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;

        explosion = Resources.Load<GameObject>("Explosion");
    }


    private void Update()
    {
        Trajectory();

        if(!dropOff)
        {
            CalculateWhenDropOff();
        }
        else
        {
            RotateDownwards();
        }

        rb.velocity = velocity;
    }

    private void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * missleSpeed;
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
            collided = true; //needed that no double collissios lead to double explosions
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Instantiate(explosion, position, rotation);
            Destroy(gameObject);
        }
    }
}
