using UnityEngine;

class Infinity : MonoBehaviour, IGameState
{
    public Infinity()
    {
        Debug.Log($"GameState: {GetType()}");
    }
}
