using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    public float health { get; private set; }
    [SerializeField]
    float maxHealth, healthShow;

    private void Awake()
    {
        health = maxHealth;
        healthShow = health;

        SetUi();
    }

    public void AddHealth(float amount)
    {
        health += amount;

        if (health >= maxHealth)
            health = maxHealth;

        healthShow = health;
        SetUi();
    }

    public void RemoveHealth(float amount)
    {
        health -= amount;

        if (health <= 0)
            health = 0;

        healthShow = health;
        SetUi();
    }

    void SetUi()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }
}
