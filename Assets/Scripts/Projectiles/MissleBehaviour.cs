using System.Collections;
using UnityEngine;

public class MissleBehaviour : ProjectileBehaviour
{
    //Gameobjects and components
    private GameObject explosion;

    private bool collided = false;

    private bool dropOff = false;
    private Vector3 startPosition;

    private bool isDoubleExplosion = false;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;

        explosion = Resources.Load<GameObject>("Projectile/Explosion");

        //Missle values
        ProjectileSpeed = 12f;
        DropOffDistance = 30f;
        DropOffSpeed = 0.04f;
        LifeSpan = 25f;
        Damage = UpgradeManager.Instance.PlayerMissleDamage;
        isDoubleExplosion = UpgradeManager.Instance.DoubleExplosion;

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
    protected override void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            collided = true; //needed that no double collissions lead to double explosions because of rockets bigger models
            ContactPoint contact = collision.contacts[0];
            Instantiate(explosion, contact.point, Quaternion.identity);
            if (isDoubleExplosion)
            {
                StartCoroutine(DelayedExplosion(contact.point));
                return;
            }
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

    /// <summary>
    /// Delays the second explosion for the double explosion upgrade
    /// </summary>
    /// <param name="contactPoint"></param>
    /// <returns></returns>
    private IEnumerator DelayedExplosion(Vector3 contactPoint)
    {
        yield return new WaitForSeconds(0.2f);

        Instantiate(explosion, contactPoint, Quaternion.identity);
        Destroy(gameObject);
    }
}
