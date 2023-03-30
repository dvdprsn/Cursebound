using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 50f;
    private float currentHealth;
    private bool isDead = false;
    public float soulValue = 5f;
    private AIController controller;

    public float getSoulValue()
    {
        return soulValue;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        controller = GetComponent<AIController>();
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Trigger death animation
            controller.IsDead = true;
            isDead = true;

        }
    }
}
