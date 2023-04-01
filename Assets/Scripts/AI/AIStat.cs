using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStat : MonoBehaviour
{
    // Start is called before the first frame update

    public float health;
    public float dmg;
    public float soulValue;
    private bool isDead;
    public float Dmg => dmg;
    public float Health => health;

    public void ApplyDamage(float up)
    {
        health -= up;
        if (health <= 0) isDead = true;
    }

    public float SoulValue => soulValue;
    
    public bool IsDead
    {
        get { return isDead;  }
        set { isDead = value; }
    }
    public void ChangeDifficulty(float multiplier)
    {
        health *= multiplier;
        dmg *= multiplier;
    }
}
