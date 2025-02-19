using UnityEngine;

public class EnemyTankV1 : EnemyBehaviour
{

    protected override void Awake()
    {
        base.Awake();
        EnemyType = EnemyType.Moving;
        Projectile = Resources.Load<GameObject>("Projectile/EnemyProjectile1");
        enemyStats = Resources.Load<EnemyStats>("EnemyStats/Normal");
        GetStats();
    }

    protected override void Update()
    {
        base.Update();
    }
}
