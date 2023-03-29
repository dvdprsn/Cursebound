using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	
	private float health = 150.0f;
	private float maxHealth = 150.0f;

	[SerializeField] private float dmgMulti = 1f;
	[SerializeField] private float maxManaMul = 1f;
	[SerializeField] private float manaRechargeMul = 1f;
	[SerializeField] private float timeToCastMul = 1f;

	[SerializeField] private float soulBalance = 0f;

	private bool dead = false;

	private ThirdPersonScript controller;
	private Transform spawnPoint;

	public void AddSouls(float souls)
    {
		soulBalance += souls;
    }

	public float GetDmgMul()
    {
		return dmgMulti;
    }

	public void AddHealth(float moreHealth){
		health += moreHealth;
	}
	
	public float GetHealth(){
		return health;
	}

    void Start()
    {
		controller = GetComponent<ThirdPersonScript>();
    }


    public bool isAlive() {return !dead;}
	
	public void ApplyDamage(float damage){
		health -= damage;
		//Debug.Log("Ouch! " + health);
        if (health <= 0){
			health = 0;
			StartCoroutine(Die());
		}
	}
    
	IEnumerator Die(){
		dead = true;
		print("Dead!");

		// Tele back to spawn point
		controller.MoveTo(spawnPoint.position);

		// Remove all enemies
		// Spawn all enemies
		// Show shop
		// Release cursor
		// Once shop is closed , lock cursor and hide ui

		yield return new WaitForSeconds(10);
		print("Alive!");
		health = maxHealth;
		dead = false;
	}
	
}
