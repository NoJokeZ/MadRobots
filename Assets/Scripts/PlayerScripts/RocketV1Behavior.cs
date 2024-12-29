using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketV1Behavior : PlayerBehavior
{
    //Cooldown values
    private float cooldown;
    private float shootCooldown = 2f;

    //Bullet prefab
    private GameObject missle;

    protected override void Awake()
    {
        base.Awake();

        missle = Resources.Load<GameObject>("Missle");
    }

    protected override void Update()
    {
        base.Update();
        launchRocket();
        CooldownCountDown();

    }

    private void launchRocket()
    {
        if(shoot.WasPressedThisFrame() && cooldown == 0f)
        {
            cooldown = shootCooldown;

            Transform useBarrel = barrels[barrelsIndex];
            barrelsIndex++;
            if (barrelsIndex >= barrels.Length)
            {
                barrelsIndex = 1;
            }

            Instantiate(missle, useBarrel.position, cameraTransform.rotation);

        }
    }

    private void CooldownCountDown()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0f;
        }
    }
}
