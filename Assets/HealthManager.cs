using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public float health { get; private set; }
    [SerializeField]
    float maxHealth, healthShow;

    private void Awake()
    {
        health = maxHealth;
        healthShow = health;
    }

    public void AddHealth(float amount)
    {
        health += amount;

        if (health >= maxHealth)
            health = maxHealth;

        healthShow = health;
    }

    public void RemoveHealth(float amount)
    {
        health -= amount;

        if (health <= 0)
            health = 0;

        healthShow = health;
    }
}
