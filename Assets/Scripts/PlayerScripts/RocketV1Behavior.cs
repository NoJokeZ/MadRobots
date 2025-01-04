using System.Collections;
using UnityEngine;

public class RocketV1Behavior : PlayerBehavior
{
    //Cooldown values
    private float cooldown;
    private float shootCooldown = 0.2f;

    //Bullet prefab
    private GameObject missle;
    private GameObject missleTD;

    //WeaponTurn values
    private Transform weapons;
    private Transform weaponsFPSave;
    private Quaternion weaponsUp;
    private Quaternion weaponsDown;

    public float moveDirectionZ;
    public float moveDirectionX;


    protected override void Awake()
    {
        base.Awake();

        missle = Resources.Load<GameObject>("Missle");
        missleTD = Resources.Load<GameObject>("missleTD");

        weapons = transform.Find("Weapons");

    }

    protected override void Update()
    {
        base.Update();
        launchRocket();
        CooldownCountDown();

        if (topDownAbility.WasPressedThisFrame() && !isTopDown && !isTransitionOngoing)
        {
            StartCoroutine(RotateWeaponsToTD());
        }
        else if (topDownAbility.WasPressedThisFrame() && isTopDown && !isTransitionOngoing)
        {
            StartCoroutine(RotateWeaponsToFP());
        }

        //Debug
        moveDirectionZ = move.ReadValue<Vector2>().y;
        moveDirectionX = move.ReadValue<Vector2>().x;

    }

    private void launchRocket()
    {
        if (shoot.WasPressedThisFrame() && cooldown == 0f)
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

}
