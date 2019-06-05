using UnityEngine;

public class JamState : TowerState
{
    public float JamTime;

    [Header("Jam effect")]
    public ParticleSystem JamEffect;

    void OnEnable()
    {
        JamEffect.Play();

        CancelInvoke("Unjam");
        Invoke("Unjam", JamTime);
    }

    private void Unjam()
    {
        SetTowerState(Tower.IdleState);
    }
}

