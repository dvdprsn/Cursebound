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
    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = spellToCast.SpellRadius;

        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.isKinematic = true;

        Destroy(this.gameObject, spellToCast.LifeTime);

    }
    private void Update()
    {
        if (spellToCast.Speed > 0) transform.Translate(Vector3.forward * spellToCast.Speed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            // Could add player dmg multiplier here!
            enemyHealth.TakeDamage(spellToCast.Damage);
        }
        Destroy(this.gameObject);
    }
}
