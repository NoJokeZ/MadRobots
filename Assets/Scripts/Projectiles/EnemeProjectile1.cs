public class EnemeProjectile1 : ProjectileBehavior
{

    protected override void Awake()
    {
        base.Awake();

        ProjectileSpeed = 9f;
        LifeSpan = 25f;
    }
}
