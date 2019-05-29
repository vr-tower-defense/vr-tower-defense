public class JamState : TowerState
{
    public float JamTime;
    
    void OnEnable()
    {
        Invoke("Unjam", JamTime);
    }

    private void Unjam()
    {
        SetTowerState(Tower.IdleState);
    }
}

