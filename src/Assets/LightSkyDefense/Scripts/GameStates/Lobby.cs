using UnityEngine;

class Lobby : MonoBehaviour, IGameState
{
    public Lobby()
    {
        Debug.Log($"GameState: {GetType()}");
    }
}
