using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeProjectile1 : ProjectileBehavior
{

    protected override void Awake()
    {
        base.Awake();

        ProjectileSpeed = 4f;
        LifeSpan = 30f;
        Damage = 3;
    }

}
