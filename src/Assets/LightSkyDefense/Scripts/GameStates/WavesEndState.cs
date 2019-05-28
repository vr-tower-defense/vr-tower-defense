using UnityEngine;

class WavesEndState : GameState
{
    public GameState WinState;

    private void FixedUpdate()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length < 1)
        {
            SetGameState(WinState);
        }
    }
}
