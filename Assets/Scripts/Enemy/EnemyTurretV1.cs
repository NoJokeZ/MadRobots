using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTurretV1 : EnemyBehavior
{

    private Transform upperBody;
    private Transform weapon;
    private Transform weaponEnd;

    private float rotateSmoothness = 20f;
    private float rotateWeaponSmothness = 30f;

    private float detectionRange = 10f;

    private int playerLayerMaskIndex;

    bool playerDetected = false;
    bool playerOnceSeen = false;

    private Vector3 playerLastSeenPostion;

    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 10;
        upperBody = transform.Find("Body2");
        weapon = transform.Find("Body2/Weapon");
        weaponEnd = transform.Find("Body2/Weapon/WeaponEnd");
        playerLayerMaskIndex = LayerMask.NameToLayer("Player");
    }

    protected override void Update()
    {
        base.Update();
        CheckCanSeePlayer();
        if (playerOnceSeen)
        {
            RotateTowardsPlayer();
        }

    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);

        float angle2 = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection2 = Quaternion.AngleAxis(angle2, Vector3.left);
        weapon.rotation = Quaternion.RotateTowards(weapon.rotation, newDirection2, rotateWeaponSmothness * Time.deltaTime);
        weapon.eulerAngles = new Vector3(weapon.eulerAngles.x, upperBody.eulerAngles.y, upperBody.eulerAngles.z);

    }

    private void CheckCanSeePlayer()
    {
        Vector3 playerDirection = (player.transform.position - upperBody.position).normalized;
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
        }
    }
}
