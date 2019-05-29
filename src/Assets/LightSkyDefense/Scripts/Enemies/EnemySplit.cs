﻿using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    public GameObject NewEnemy;
    public int SplitAmount = 2;

    void OnDestroy()
    {
        for (int i = 0; i < SplitAmount; i++)
        {
            Instantiate(
                NewEnemy,
                transform.position,
                Random.rotation
            );
        }
    }
}
