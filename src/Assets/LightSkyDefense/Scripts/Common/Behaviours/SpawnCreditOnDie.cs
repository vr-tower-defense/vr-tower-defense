using UnityEngine;

public class SpawnCreditOnDie : MonoBehaviour
{
    public Credit Credit;
    public float Value;

    /// <summary>
    /// Spawn money when its gameObject is destroyed.
    /// </summary>
    void OnDie()
    {
        var clone = Instantiate(
            Credit,
            gameObject.transform.position,
            gameObject.transform.rotation
        );

        clone.Value = Value;
    }
}
