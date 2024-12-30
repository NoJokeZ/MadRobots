using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBehavior : EnemyBehavior
{

    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 10;
    }

}
