using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HealthBar healthbar;
    public float maxHealth = 10f;
    private float health;
    public float dmg;
    public float soulValue;
    private bool isDead;
    [SerializeField]
    private float runSpeed = 3f;
    [SerializeField]
    private float walkSpeed = 1.8f;
    public float Dmg => dmg;
    public float Health => health;
    private void Awake()
    {
        health = maxHealth;
        healthbar.UpdateHealthBar(maxHealth, health);
    }
    public void ApplyDamage(float up)
    {
        health -= up;
        healthbar.UpdateHealthBar(maxHealth, health);
        if (health <= 0) isDead = true;
    }

    public float SoulValue => soulValue;
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    
    public bool IsDead
    {
        get { return isDead;  }
        set { isDead = value; }
    }
    public void ChangeDifficulty(float multiplier)
    {
        maxHealth *= multiplier;
        health = maxHealth;
        dmg *= multiplier;
    }
}
