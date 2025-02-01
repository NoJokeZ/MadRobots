public class IceSpikesManager : EnemyManager
{
    protected override void Awake()
    {
        dummyAmount = 0;
        stationaryAmount = 1;
        movingAmount = 1;
        bossAmount = 0;
        base.Awake();
    }
}
