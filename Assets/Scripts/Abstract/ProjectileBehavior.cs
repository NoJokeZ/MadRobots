using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{

    protected float lifeSpan;
    public int Damage { get; protected set; }


    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        lifeSpan = lifeSpan - Time.deltaTime;

        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
