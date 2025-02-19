using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    //Projectile values
    public float ProjectileSpeed { get; protected set; }
    public float DropOffDistance { get; protected set; }
    public float DropOffSpeed { get; protected set; }
    public float LifeSpan { get; protected set; }
    public float Damage { get; set; }

    //Velocity
    protected Vector3 velocity;

    //Gameobjects and components
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected virtual void Update()
    {
        LifeSpanCheck();

        Trajectory();

        rb.velocity = velocity;
    }

    /// <summary>
    /// LifeSpan calculation
    /// </summary>
    private void LifeSpanCheck()
    {
        LifeSpan -= Time.deltaTime;

        if (LifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Trajectory calculation
    /// </summary>
    protected virtual void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * ProjectileSpeed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
