using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    //Projectile values
    public float ProjectileSpeed { get; protected set; }
    public float DropOffDistance { get; protected set; }
    public float DropOffSpeed { get; protected set; }
    public float LifeSpan { get; protected set; }
    public int Damage { get; protected set; }

    //Gameobjects and components
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected virtual void Update()
    {
        LifeSpanCheck();
    }

    /// <summary>
    /// LifeSpan calculation
    /// </summary>
    private void LifeSpanCheck()
    {
        LifeSpan -= Time.deltaTime;

        if (LifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
