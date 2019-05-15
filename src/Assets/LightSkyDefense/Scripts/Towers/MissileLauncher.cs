public class MissileLauncher : BaseTower
{
    public MissileLauncher()
    {
        IdleState = typeof(IdleRotationState);
        ActiveState = typeof(IdleRotationState);
        CelebrationState = typeof(CelebrationState);
    }
}
