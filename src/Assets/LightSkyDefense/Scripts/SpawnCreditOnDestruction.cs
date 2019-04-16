using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreditOnDestruction : MonoBehaviour
{
    public Credit Credit;
    public int Value;

    private bool isQuitting = false;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    /// <summary>
    /// Spawn money when its gameObject is destroyed.
    /// </summary>
    void OnDisable()
    {
        Credit.Value = Value;
        Instantiate(Credit, gameObject.transform.position, gameObject.transform.rotation);
        if (!isQuitting)
        {

            Credit.Value = Value;
            Instantiate(Credit, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
