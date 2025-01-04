using UnityEngine;

public class MissleBehavior : ProjectileBehavior
{
    //Gameobjects and components
    private GameObject explosion;

    private bool collided = false;

    private bool dropOff = false;
    private Vector3 startPosition;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;

        explosion = Resources.Load<GameObject>("Explosion");

        //Missle values
        ProjectileSpeed = 7f;
        DropOffDistance = 20f;
        DropOffSpeed = 0.06f;
        LifeSpan = 20f;
        Damage = 1;

    }


    protected override void Update()
    {
        base.Update();

        DropOffCheck();

    }

    /// <summary>
    /// Creates explostion on impact
    /// </summary>
    /// <param name="collision"></param>
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
    protected override void Trajectory()
    {
        Vector3 trajectoryDirection = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        velocity = trajectoryDirection * ProjectileSpeed;
    }

    /// <summary>
    /// Applies projectile drop off
    /// </summary>
    private void RotateDownwards()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + DropOffSpeed, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// Calculates when projectile drop off starts
    /// </summary>
    private void CalculateWhenDropOff()
    {
        Vector3 currentPostion = transform.position;
        float distance = Vector3.Distance(startPosition, currentPostion);

        if (distance > DropOffDistance)
        {
            dropOff = true;
        }
    }
}
