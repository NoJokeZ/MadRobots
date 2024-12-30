using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissleBehavior : ProjectileBehavior
{
    protected override void Awake()
    {
        base.Awake();

        //Missle values
        projectileSpeed = 7f;
        dropOffDistance = 20f;
        dropOffSpeed = 0.06f;

    }
}
