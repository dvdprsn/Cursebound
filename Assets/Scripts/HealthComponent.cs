using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 50f;
    private float currentHealth;
    private bool isDead = false;
    public float soulValue = 5f;

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
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;
            //Destroy(this.gameObject);
        }
    }
}
