using System.Collections;
using UnityEngine;

public class RocketV1Behaviour : PlayerBehaviour
{

    //Bullet prefab
    private GameObject missle;
    private GameObject missleTD;

    protected override void Awake()
    {
        base.Awake();

        myType = PlayerType.Rocket;

        missle = Resources.Load<GameObject>("Projectile/Missile");
        missleTD = Resources.Load<GameObject>("Projectile/MissleTD");

    }

    protected override void Update()
    {
        base.Update();
        LaunchRocket();
        CooldownCountDown();

        if (topDownAbility.WasPressedThisFrame() && !isTopDown && !isTransitionOngoing)
        {
            specialAbilityAmmoCount = specialAbilityAmmo;
            StartCoroutine(RotateWeaponsToTD());
            StartCoroutine(cameraBehavior.TransitionToTD());
        }
        else if (topDownAbility.WasPressedThisFrame() && isTopDown && !isTransitionOngoing)
        {
            StartCoroutine(RotateWeaponsToFP());
            StartCoroutine(cameraBehavior.TransitionToFP());
        }
    }

    private void LaunchRocket()
    {
        if (shoot.WasPressedThisFrame() && shootCooldown == 0f)
        {
            shootCooldown = firrate;

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

                Vector3 inverseCameraRoation = new Vector3(cameraTransform.eulerAngles.x * -1, cameraTransform.eulerAngles.z, cameraTransform.eulerAngles.y); //I don't know why it works like that... but it does

                Instantiate(missleTD, useBarrel.position, Quaternion.Euler(inverseCameraRoation));
                specialAbilityAmmoCount--;
                if (specialAbilityAmmoCount == 0)
                {
                    StartCoroutine(RotateWeaponsToFP());
                    StartCoroutine(cameraBehavior.TransitionToFP());
                }

            }

        }
    }

    private void CooldownCountDown()
    {
        if (shootCooldown > 0f)
        {
            shootCooldown -= Time.deltaTime;
        }
        else
        {
            shootCooldown = 0f;
        }
    }

    private IEnumerator RotateWeaponsToTD()
    {

        //weaponsFPSave.rotation = weapons.rotation; //maybe need this

        float t = 0f;

        //Transition
        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / 1);

            weapons.rotation = Quaternion.Lerp(weapons.rotation, Quaternion.Euler(new Vector3(-90f, weapons.eulerAngles.y, weapons.eulerAngles.z)), t);

            yield return null;
        }
    }

    private IEnumerator RotateWeaponsToFP()
    {

        //weaponsFPSave.rotation = weapons.rotation; //maybe need this

        float t = 0f;

        //Transition
        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / 1);

            weapons.rotation = Quaternion.Lerp(weapons.rotation, Quaternion.Euler(new Vector3(0f, weapons.eulerAngles.y, weapons.eulerAngles.z)), t);

            yield return null;
        }
    }

    protected override void GetPlayerStats()
    {
        base.GetPlayerStats();
    }

}
