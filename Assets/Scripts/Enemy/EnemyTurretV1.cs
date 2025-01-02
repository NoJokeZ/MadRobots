using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTurretV1 : EnemyBehavior
{
    //GameObjects and components
    private Transform upperBody;
    private Transform weapon;
    private Transform weaponEnd;


    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 15;
        upperBody = transform.Find("Body2");
        weapon = transform.Find("Body2/Weapon");
        weaponEnd = transform.Find("Body2/Weapon/WeaponEnd");
        projectile = Resources.Load<GameObject>("EnemyeProjectile1");
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Rotated upper body and weapon towards the player 
    /// </summary>
    protected override void RotateTowardsPlayer()
    {
        //Rotation of upper body towards player
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);

        //Rotation of weapon towards player
        Vector3 direction2 = (playerLastSeenPostion - weapon.position).normalized;
        Quaternion newDirection2 = Quaternion.LookRotation(direction2, Vector3.up);
        weapon.rotation = Quaternion.RotateTowards(weapon.rotation, newDirection2, rotateWeaponSmothness * Time.deltaTime);
        weapon.eulerAngles = new Vector3(weapon.eulerAngles.x, upperBody.eulerAngles.y, upperBody.eulerAngles.z);

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
        Vector3 lookDirection = weapon.transform.forward;
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
            shootCooldown = shootInterval;
            
        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }
}
