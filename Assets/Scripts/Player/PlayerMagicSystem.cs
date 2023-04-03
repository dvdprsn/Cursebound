using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSystem : MonoBehaviour
{

    [SerializeField] public Spell spellToCast;
    [SerializeField] private Transform castPoint;

    private bool castingMagic = false;
    private float currentCastTimer;
    public Animator animator;
    private PlayerStatus pStats;

    private void Awake()
    {
        pStats = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!pStats.isDead)
        {
            bool isCastingButtonPressed = Input.GetKey(KeyCode.Mouse0);
            bool hasMana = pStats.GetCurrentMana - spellToCast.spellToCast.ManaCost >= 0f;
            if (!castingMagic && isCastingButtonPressed && hasMana)
            {
                castingMagic = true;
                pStats.SetCurrentMana(pStats.GetCurrentMana - spellToCast.spellToCast.ManaCost);
                currentCastTimer = 0;
                animator.SetTrigger("Attack");
                CastSpell();
                
            }
            if (castingMagic)
            {
                currentCastTimer += Time.deltaTime;
                if (currentCastTimer > pStats.GetTimeToCast) castingMagic = false;
            }

            if (pStats.GetCurrentMana < pStats.GetMaxMana) pStats.SetCurrentMana(pStats.GetCurrentMana + pStats.GetManaRechargeRate * Time.deltaTime);
        }
    }
    void CastSpell()
    {
        // CAST HERE
        Spell s = Instantiate(spellToCast, castPoint.position, castPoint.rotation);
        // Set to only damage enemy type
        s.SetDmgType(1);
    }
}
