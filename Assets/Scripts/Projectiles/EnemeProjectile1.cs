public class EnemeProjectile1 : ProjectileBehaviour
{

    protected override void Awake()
    {
        base.Awake();

        ProjectileSpeed = 9f;
        LifeSpan = 25f;
    }
}
