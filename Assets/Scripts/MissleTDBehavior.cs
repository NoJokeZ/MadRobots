using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MissleTDBehavior : ProjectileBehavior
{
    //Gameobjects and components
    private GameObject explosion;

    private bool collided = false;

    private Vector3 velocity;

    private bool targetMouse = false;
    private Vector3 startPosition;
    private float targetMouseDistance = 5f;
    private float targetMouseSmothness = 15f; // Needs testing

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;

        explosion = Resources.Load<GameObject>("Explosion");

        //Missle values
        ProjectileSpeed = 2f;
        LifeSpan = 40f;
    }

    protected override void Update()
    {
        base.Update();

        Trajectory();
        FollowTargetCheck();

        rb.velocity = velocity;
    }

    private void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * ProjectileSpeed;
    }

    private void FollowTargetCheck()
    {
        if (!targetMouse)
        {
            CalculateWhenTargetMouse();
        }
        else
        {
            TargetMouse();
        }
    }

    /// <summary>
    /// Applies projectile drop off
    /// </summary>
    private void TargetMouse()
    {
        Vector3 mouseTarget = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = (mouseTarget - transform.position).normalized;
        Quaternion targetDirection = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetDirection, targetMouseSmothness * Time.deltaTime);
    }

    /// <summary>
    /// Calculates when projectile start to target mouse
    /// </summary>
    private void CalculateWhenTargetMouse()
    {
        Vector3 currentPostion = transform.position;
        float distance = Vector3.Distance(startPosition, currentPostion);

        if (distance > targetMouseDistance)
        {
            targetMouse = true;
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
