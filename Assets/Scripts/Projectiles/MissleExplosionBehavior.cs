public class MissleExplosionBehavior : ExplosionBehavior
{

    protected override void Awake()
    {
        base.Awake();
        lifeSpan = 0.5f;
        Damage = 5;
    }
}
