public class MissleExplosionBehaviour : ExplosionBehaviour
{

    protected override void Awake()
    {
        base.Awake();
        explosionCollider.radius = UpgradeManager.Instance.PlayerExplosionRadius;
        Damage = UpgradeManager.Instance.PlayerExplosionDamage;
        
    }
}
