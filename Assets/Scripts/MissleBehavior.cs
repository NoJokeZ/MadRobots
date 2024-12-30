using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissleBehavior : ProjectileBehavior
{
    //Gameobjects and components
    private GameObject explosion;


    private bool collided = false;

    protected override void Awake()
    {
        base.Awake();

        explosion = Resources.Load<GameObject>("Explosion");

        //Missle values
        ProjectileSpeed = 7f;
        DropOffDistance = 20f;
        DropOffSpeed = 0.06f;
        LifeSpan = 20f;
        Damage = 1;

    }

    /// <summary>
    /// Creates explostion on impact
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
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
}
