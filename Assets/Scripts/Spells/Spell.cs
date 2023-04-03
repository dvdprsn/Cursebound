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
    private Camera cam;
    public GameObject DmgNumPrefab;

    private int dmgType;

    public void SetDmgType(int type)
    {
        dmgType = type;
    }
    private void Awake()
    {
        cam = Camera.main;
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
        // Spell only interacts with enemy type
        if (dmgType == 1)
        {
            //If the spell hits an enemy
            if (other.gameObject.CompareTag("Enemy"))
            {
                AIStat enemyStats = other.GetComponent<AIStat>();
                float dmg = spellToCast.Damage * pStats.DmgMul;
                // Could add player dmg multiplier here!
                enemyStats.ApplyDamage(dmg);
                Debug.Log("Enemy Damaged: " + dmg);
                //Trigger floating text

                if (DmgNumPrefab != null)
                {
                    Vector3 pos = other.transform.position;
                    pos.y += 2;
                    pos.z += 1;
                    var go = Instantiate(DmgNumPrefab, pos, Quaternion.LookRotation(pos - cam.transform.position), other.transform);
                    go.GetComponent<TextMesh>().text = dmg.ToString();
                }
                // Award player souls
                if (enemyStats.IsDead) pStats.AddSouls(enemyStats.soulValue);
            }
        }
        else // Only for player
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AIStat enemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AIStat>();
                pStats.ApplyDamage(enemyStats.Dmg);
            }
        }

        // Prevents despawning in room colliders or powerups
        //Is not GameElement and is not PowerUp
        if (!other.gameObject.CompareTag("GameElement") && !other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(this.gameObject);
        }
    }

}
