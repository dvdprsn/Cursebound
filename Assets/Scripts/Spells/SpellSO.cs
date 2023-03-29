using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellSO : ScriptableObject
{
    // Start is called before the first frame update
    public float ManaCost = 5f;
    public float LifeTime = 2f;
    public float Speed = 15f;
    public float SpellRadius = 0.5f;
    public float Damage = 10f;

    //Status effect
    // thumbnail
    // time between casts
    // magic elements 
}
