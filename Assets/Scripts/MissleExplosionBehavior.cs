using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissleExplosionBehavior : ExplosionBehavior
{

    protected override void Awake()
    {
        base.Awake();
        lifeSpan = 0.5f;
        Damage = 5;
    }
}
