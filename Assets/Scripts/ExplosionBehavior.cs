using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionBehavior : ProjectileBehavior
{

    protected override void Awake()
    {
        base.Awake();
        lifeSpan = 3f;
        Damage = 5;
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
