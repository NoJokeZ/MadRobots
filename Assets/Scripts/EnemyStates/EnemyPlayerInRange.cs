using System.Collections;
using UnityEngine;

public class EnemyPlayerInRange : EnemyState
{
    [SerializeField] EnemyState EnemyIdleState;
    [SerializeField] EnemyState EnemyPlayerInRangeState;

    private bool isMoving;
    private float playerOutOfSightTimer;
    private float playerOutOfSightWaitTime = 5f;

    private bool isLowHealth = false;

    private float followDistance = 20f;
    private float backOffDistance = 10f;

    protected override void Awake()
    {
        base.Awake();

        this.enabled = false;
    }


    protected override void Update()
    {
        base.Update();
        CheckPlayer();
        if (playerDetected)
        {
            CheckShoot();
            CheckMove();
            RotateBottomTowardsPlayer();
        }

        if (playerOnceSeen)
        {
            RotateTopAndWeaponTowardsPlayer();
        }

        if (startShoot)
        {
            BurstShoot();
        }

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isMoving)
        {
            velocity = rb.velocity;
        }

    }

    protected override IEnumerator OneSecUpdate()
    {
        while (true)
        {
            CheckHealth();

            yield return new WaitForSeconds(1f);
        }
    }

    private void CheckPlayer()
    {
        playerDirection = (player.transform.position - upperBody.position).normalized;

        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex)
            {
                playerDetected = true;
                playerOnceSeen = true;
                playerLastSeenPostion = hitInfo.transform.position;
                playerOutOfSightTimer = playerOutOfSightWaitTime;
            }
            else
            {
                playerDetected = false;
                startShoot = false;
                playerOutOfSightTimer -= Time.deltaTime;
                if (playerOutOfSightTimer < 0)
                {
                    EnemyIdleState.enabled = true;
                    EnemyPlayerInRangeState.enabled = false;
                }
            }
        }
        else
        {
            playerDetected = false;
            startShoot = false;
            playerOutOfSightTimer -= Time.deltaTime;
            if (playerOutOfSightTimer < 0)
            {
                EnemyIdleState.enabled = true;
                EnemyPlayerInRangeState.enabled = false;
            }
        }
    }

    private void CheckShoot()
    {
        Vector3 lookDirection = weaponEnd.transform.forward;
        float angle = Vector3.Angle(lookDirection, playerDirection);

        if (angle <= startShootAngle)
        {
            startShoot = true;
        }
        else
        {
            startShoot = false;
        }
    }

    private void CheckMove()
    {
        float distanceToPlayer = (playerLastSeenPostion - transform.position).magnitude;

        if (!isLowHealth && distanceToPlayer > followDistance)
        {
            isMoving = true;
            Move(Direction.forward);
        }
        else if (isLowHealth || distanceToPlayer < backOffDistance)
        {
            isMoving = true;
            Move(Direction.backward);
        }
        else
        {
            isMoving = false;
        }
    }

    private void CheckHealth()
    {
        if ((enemyBehavior.EnemyMaxHealth / enemyBehavior.EnemyHealth) > 0.5f)
        {
            isLowHealth = true;
        }
        else
        {
            isLowHealth = false;
        }
    }
}
