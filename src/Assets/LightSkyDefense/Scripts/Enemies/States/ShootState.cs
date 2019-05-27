﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootState : EnemyState
{
    public Rigidbody Projectile;
    public float ShootInterval = 6;
    public float LoadUpTime = 5;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Shoot(ShootInterval));
    }

    private void FixedUpdate()
    { 
    }

    private void OnDisable()
    {
        StopCoroutine(Shoot(ShootInterval));
    }

    private IEnumerator Shoot(float interval)
    {
        yield return new WaitForSeconds(LoadUpTime);

        while (true)
        {
            ShootAtTower();
            yield return new WaitForSeconds(interval);
        }
    }

    private void ShootAtTower()
    {
        var rand = Random.Range(0, Enemy.TowersInRange.Count);
        var tower = Enemy.TowersInRange[rand];
        var target = tower.transform.position;

        var newProjectile = Instantiate(
            Projectile,
            transform.position,
            transform.rotation
        );

        newProjectile.velocity = target - transform.position;
    }
}