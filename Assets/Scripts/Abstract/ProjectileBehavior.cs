using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    //Projectile values
    public float projectileSpeed { get; protected set; }
    public float dropOffDistance { get; protected set; }
    public float dropOffSpeed { get; protected set; }
    public float lifeSpan { get; protected set; }
    public int Damage { get; protected set; }

    //Trajectory values
    private Vector3 velocity;
    private bool dropOff = false;
    private Vector3 startPosition;

    //Gameobjects and components
    private Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }


    protected virtual void Update()
    {
        Trajectory();

        DropOffCheck();

        rb.velocity = velocity;

        LifeSpanCheck();
    }

    /// <summary>
    /// LifeSpan calculation
    /// </summary>
    private void LifeSpanCheck()
    {
        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks projectile drop off
    /// </summary>
    private void DropOffCheck()
    {
        if (!dropOff)
        {
            CalculateWhenDropOff();
        }
        else
        {
            RotateDownwards();
        }
    }

    /// <summary>
    /// Porjectile trajectory calucaltion
    /// </summary>
    private void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * projectileSpeed;
    }

    /// <summary>
    /// Applies projectile drop off
    /// </summary>
    private void RotateDownwards()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + dropOffSpeed, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// Calculates when projectile drop off starts
    /// </summary>
    private void CalculateWhenDropOff()
    {
        Vector3 currentPostion = transform.position;
        float distance = Vector3.Distance(startPosition, currentPostion);

        if (distance > dropOffDistance)
        {
            dropOff = true;
        }
    }
}
