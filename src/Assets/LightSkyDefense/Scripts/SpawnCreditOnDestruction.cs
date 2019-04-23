using UnityEngine;

public class SpawnCreditOnDestruction : MonoBehaviour
{
    public Credit Credit;
    public int Value;

    private bool _isQuitting = false;

    void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    /// <summary>
    /// Spawn money when its gameObject is destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (!_isQuitting)
        {
            Credit.Value = Value;
            Instantiate(Credit, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
