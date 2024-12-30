using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    public int HealtPoints { get; protected set; } = 10; //Default HP
    private bool isExplosionDamageDelay = false;
    private float explosionDamageDelayTime = 0.25f;
    private float explosionDamageDeleyCounter = 0f;


    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        CheckHP();
        if (isExplosionDamageDelay)
        {
            ExplosionDamageDelayCountdown();
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        HealtPoints -= damage;
    }

    /// <summary>
    /// Checks HP and destroys itself
    /// </summary>
    private void CheckHP()
    {
        if (HealtPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Check for projectile hits
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            int damage = collision.gameObject.GetComponent<ProjectileBehavior>().Damage;
            TakeDamage(damage);
        }
    }

    /// <summary>
    /// Check for explosion hits
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Explosion") && !isExplosionDamageDelay)
        {
            isExplosionDamageDelay = true;
            explosionDamageDeleyCounter = explosionDamageDelayTime;
            int damage = other.gameObject.GetComponent<ExplosionBehavior>().Damage;
            TakeDamage(damage);
        }
    }

    private void ExplosionDamageDelayCountdown()
    {
        explosionDamageDeleyCounter -= Time.deltaTime;
        if (explosionDamageDeleyCounter <= 0)
        {
            isExplosionDamageDelay = false;
        }
    }
}
