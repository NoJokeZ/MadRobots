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
    private GameObject missleTD;


    protected override void Awake()
    {
        base.Awake();

        missle = Resources.Load<GameObject>("Missle");
        missleTD = Resources.Load<GameObject>("missleTD");
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

            if (!isTopDown)
            {
                Instantiate(missle, useBarrel.position, cameraTransform.rotation);
            }
            else
            {
                //float x = cameraTransform.rotation.x * -1;
                //float y = cameraTransform.rotation.y * -1;
                //float z = cameraTransform.rotation.z * -1;
                //Vector3 inverseCameraRotation = new Vector3(x, y, z);

                Vector3 inverseCameraRoation = new Vector3(cameraTransform.eulerAngles.x * -1, cameraTransform.eulerAngles.z, cameraTransform.eulerAngles.y); //I don't know why it works like that... but it does

                Instantiate(missleTD, useBarrel.position, Quaternion.Euler(inverseCameraRoation));
            }

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
