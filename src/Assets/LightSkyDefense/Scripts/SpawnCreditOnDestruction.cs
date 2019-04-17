using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreditOnDestruction : MonoBehaviour
{
    public Credit credit;
    public int value;

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
            credit.value = value;
            Instantiate(credit, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
