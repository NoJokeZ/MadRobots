using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankV1 : EnemyBehavior
{
    private Transform upperBody;
    // Transform weapon;
    private Transform weaponEnd;

    private float burstShootCooldown = 0.2f;
    private int shotCount = 0;
    private int burstShots = 3;

    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 15;
        upperBody = transform.Find("UpperBody");
        // = transform.Find("UpperBody/Barrel");
        weaponEnd = transform.Find("UpperBody/Barrel/WeaponEnd");
        projectile = Resources.Load<GameObject>("EnemyeProjectile1");
        shootInterval = 4f;
        startShootAngle = 30f;
    }

    protected override void Update()
    {
        base.Update();
    }
















    /// <summary>
    /// Rotated upper body towards the player 
    /// </summary>
    protected override void RotateTowardsPlayer()
    {
        //Rotation of upper body towards player
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    protected override void CheckCanSeePlayer()
    {
        playerDirection = (player.transform.position - upperBody.position).normalized;
        Debug.DrawRay(upperBody.position, playerDirection * detectionRange, Color.red);

        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex)
            {
                playerDetected = true;
                playerOnceSeen = true;
                playerLastSeenPostion = hitInfo.transform.position;
            }
        }
        else
        {
            playerDetected = false;
            startShoot = false;
        }
    }

    protected override void CheckShouldShoot()
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

    protected override void Shoot()
    {
        if (shootCooldown == 0)
        {
            Instantiate(projectile, weaponEnd.position, weaponEnd.rotation);
            shotCount++;
            if(shotCount < burstShots)
            {
                shootCooldown = burstShootCooldown;
            }
            else
            {
                shootCooldown = shootInterval;
                shotCount = 0;
            }

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }
}
