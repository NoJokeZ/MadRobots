using UnityEngine;

public class MissleTDBehavior : ProjectileBehavior
{
    //Gameobjects and components
    private GameObject explosion;

    private bool collided = false;

    private bool targetMouse = false;
    private Vector3 startPosition;
    private float targetMouseDistance = 4f;
    private float targetMouseSmothness = 120f; // Needs testing

    private Vector3 screenPosition;
    private Vector3 targetPosition;
    private RaycastHit hitData;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;

        explosion = Resources.Load<GameObject>("Explosion");

        //Missle values
        ProjectileSpeed = 6f;
        LifeSpan = 40f;
        Damage = UpgradeManager.Instance.PlayerMissleDamage;

        GetTargetLocation();
    }

    protected override void Update()
    {
        base.Update();

        FollowTargetCheck();
    }

    protected override void Trajectory()
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

        Vector3 direction = (targetPosition - transform.position).normalized;

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

    protected override void OnCollisionEnter(Collision collision)
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

    private void GetTargetLocation()
    {
        screenPosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out hitData))
        {
            targetPosition = hitData.point;
        }
    }
}
