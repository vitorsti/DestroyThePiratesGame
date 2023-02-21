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
    [Header("----- Debug -----")]
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private bool takeDamage;
    [SerializeField]
    private float healAmount;
    [SerializeField]
    private bool giveHealth;

    private void OnValidate()
    {
        if (takeDamage)
        {
            RemoveHealth(damageAmount);
            takeDamage = false;
        }

        if (giveHealth)
        {
            AddHealth(healAmount);
            giveHealth = false;
        }
    }

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
            health = -1;

        healthShow = health;
        SetUi();
        float healthPercentage = (health / maxHealth) * 100;
        Debug.Log(this.gameObject.name + " " + healthPercentage);
    }
    public void ResetHealth()
    {
        health = maxHealth;
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
