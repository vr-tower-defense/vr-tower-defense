using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJammerProjectile : MonoBehaviour
{
    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 3;

    public float JamTime = 4;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeAlive);
    }

    void OnCollisionEnter(Collision collision)
    {
        var tower = collision
            .gameObject
            .GetComponent<BaseTower>();

        if (tower != null)
        {
            tower.SendMessage(
                "OnJam",
                JamTime,
                SendMessageOptions.RequireReceiver
            );
        }

        Destroy(gameObject);
    }
}
