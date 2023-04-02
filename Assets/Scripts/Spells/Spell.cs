using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    // Start is called before the first frame update
    public SpellSO spellToCast;
    private SphereCollider myCollider;
    private Rigidbody myRigidBody;
    private PlayerStatus pStats;

    private int dmgType;

    public void SetDmgType(int type)
    {
        dmgType = type;
    }
    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = spellToCast.SpellRadius;
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.isKinematic = true;
        pStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        Destroy(this.gameObject, spellToCast.LifeTime);

    }
    private void Update()
    {
        if (spellToCast.Speed > 0) transform.Translate(Vector3.forward * spellToCast.Speed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        //Enemy
        if(dmgType == 1)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                AIStat enemyStats = other.GetComponent<AIStat>();
                // Could add player dmg multiplier here!
                enemyStats.ApplyDamage(spellToCast.Damage * pStats.DmgMul);
                // Award player souls
                if (enemyStats.IsDead) pStats.AddSouls(enemyStats.soulValue * pStats.SoulMul);
            }
        } else // Player
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AIStat enemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIStat>();
                pStats.ApplyDamage(enemyStats.Dmg);
            }
        }
        
        if (!other.gameObject.CompareTag("GameElement"))
        {
            Destroy(this.gameObject);
        }
    }

}
