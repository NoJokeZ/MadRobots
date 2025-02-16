using UnityEngine;

public class EnemyStationary : EnemyState
{

    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Update()
    {
        base.Update();
        CheckPlayer();

        if (playerDetected)
        {
            RotateTopAndWeaponTowardsPlayer();
            CheckShoot();
        }

        if (startShoot)
        {
            Shoot();
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
            }
            else
            {
                playerDetected = false;
                startShoot = false;
            }
        }
        else
        {
            playerDetected = false;
            startShoot = false;
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


}
