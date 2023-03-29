using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSystem : MonoBehaviour
{

    [SerializeField] public Spell spellToCast;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana;
    [SerializeField] private float manaRecharageRate = 2f;
    [SerializeField] private float timeToCast = 0.25f;

    [SerializeField] private Transform castPoint;
    private bool castingMagic = false;
    private float currentCastTimer;
    public Animator animator;
    private void Awake()
    {
        currentMana = maxMana;
        spellToCast.pStats = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        bool isCastingButtonPressed = Input.GetKey(KeyCode.Mouse0);
        bool hasMana = currentMana - spellToCast.spellToCast.ManaCost >= 0f;
        if (!castingMagic && isCastingButtonPressed && hasMana)
        {
            castingMagic = true;
            currentMana -= spellToCast.spellToCast.ManaCost;
            currentCastTimer = 0;
            //spellToCast.spellToCast.Damage *= 
            CastSpell();
            animator.SetBool("isAttacking", true);
        }
        if(castingMagic)
        {
            currentCastTimer += Time.deltaTime;
            if (currentCastTimer > timeToCast) castingMagic = false;

        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) animator.SetBool("isAttacking", false);

        if (currentMana < maxMana) currentMana += manaRecharageRate * Time.deltaTime;
    }

    void CastSpell()
    {
        // CAST HERE
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);

    }
}
