using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSystem : MonoBehaviour
{

    [SerializeField] public Spell spellToCast;

    private PlayerStatus pStats; 

    [SerializeField] private Transform castPoint;

    private bool castingMagic = false;
    private float currentCastTimer;
    public Animator animator;
    void ResetAnimationTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(trigger.name, false);
            }
        }
    }
    private void Awake()
    {
        pStats = GetComponent<PlayerStatus>();
        pStats.SetCurrentMana(pStats.GetMaxMana);
        spellToCast.pStats = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        if(!pStats.isDead)
        {
            //ResetAnimationTriggers();

            bool isCastingButtonPressed = Input.GetKey(KeyCode.Mouse0);
            bool hasMana = pStats.GetCurrentMana - spellToCast.spellToCast.ManaCost >= 0f;
            if (!castingMagic && isCastingButtonPressed && hasMana)
            {
                castingMagic = true;
                pStats.SetCurrentMana(pStats.GetCurrentMana - spellToCast.spellToCast.ManaCost);
                currentCastTimer = 0;
                ResetAnimationTriggers();
                animator.SetBool("isAttacking", true);
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
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }
}
