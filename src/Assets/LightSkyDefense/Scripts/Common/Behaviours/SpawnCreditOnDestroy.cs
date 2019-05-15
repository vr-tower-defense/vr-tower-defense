using UnityEngine;

public class SpawnCreditOnDestroy : MonoBehaviour
{
    public Credit Credit;
    public float Value;

    /// <summary>
    /// Spawn money when its gameObject is destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (GameManager.IsQuitting)
        {
            return;
        }

        var clone = Instantiate(
            Credit, 
            gameObject.transform.position, 
            gameObject.transform.rotation
        );

        clone.Value = Value;
    }
}
