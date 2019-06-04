using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HealthBarElement;
    public GameObject Tower;

    private Damageable _damageable;

    void Start()
    {
        _damageable = Tower.GetComponent<Damageable>();

        HealthBarElement.maxValue = _damageable.MaxHealth;
        HealthBarElement.value = _damageable.InitialHealth;
    }

    void OnUpdateHealth()
    {
        HealthBarElement.value = _damageable.Health;
    }
}
