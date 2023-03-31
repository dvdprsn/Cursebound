using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	
	private float health = 150.0f;
	private float maxHealth = 150.0f;
	private int deathCount = 0;

	[SerializeField] private float dmgMulti = 1f;
	//[SerializeField] private float maxManaMul = 1f;
	//[SerializeField] private float manaRechargeMul = 1f;
	//[SerializeField] private float timeToCastMul = 1f;

	[SerializeField] private float currentMana;
	[SerializeField] private float maxMana = 100f;
	[SerializeField] private float manaRecharageRate = 2f;
	[SerializeField] private float timeToCast = 0.25f;

	[SerializeField] private float soulBalance = 0f;

	private bool dead = false;

	private ThirdPersonScript controller;
	public Transform spawnPoint;
    private void OnGUI()
    {
		GUI.Box(new Rect(Screen.width - 100, 5, 100, 50), "Heatlh: " + health.ToString() + "\n Mana: " + currentMana.ToString() + "\n Souls: " + soulBalance.ToString());
    }
    public void SetCurrentMana(float mana) => currentMana = mana;
	public float GetCurrentMana => currentMana;
    public float GetMaxMana => maxMana;
	public float GetManaRechargeRate => manaRecharageRate;
	public float GetTimeToCast => timeToCast;
	public float GetDmgMul => dmgMulti;
	public void AddSouls(float souls) => soulBalance += souls;
    public void AddHealth(float moreHealth) => health += moreHealth;
    public float Health => health;
    void Start()
    {
		controller = GetComponent<ThirdPersonScript>();
    }
    public bool isAlive() {return !dead;}
	public void ApplyDamage(float damage){
		health -= damage;
		Debug.Log("Ouch! " + health);
        if (health <= 0){
			health = 0;
			//StartCoroutine(Die());
			Die();
		}
	}
	private void DespawnEnemies()
    {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject e in enemies)
        {
			Destroy(e);
        }
    }
    public void Die()
    {
		deathCount += 1;
		dead = true;
		DespawnEnemies();
		// Tele back to spawn point
		controller.MoveTo(spawnPoint.position);


		// Remove all enemies
		// Spawn all enemies
		// Show shop
		// Release cursor
		// Once shop is closed , lock cursor and hide ui
		health = maxHealth;
		dead = false;
	}	
}
