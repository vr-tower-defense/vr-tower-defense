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
        if(Value <= 0)
            return;

        var clone = Instantiate(
            Credit,
            gameObject.transform.position,
            Quaternion.identity
        );

        clone.Value = Value;
    }
}
