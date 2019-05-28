﻿using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnStep", menuName = "Waves/SpawnStep", order = 1)]
public class SpawnStep : WaveStep
{
    public GameObject Enemy;

    public override IEnumerator Run()
    {
        yield return Instantiate(
            Enemy,
            Path.Instance[0],
            Quaternion.identity
        );
    }
}

