using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	[SerializeField]
	private float health;
	[SerializeField]
	private float maxHealth = 10.0f;
	[SerializeField]
	private int deathCount = 0;

	[SerializeField] private float dmgMulti = 1f;
	[SerializeField] private float soulMul = 1f;
	//[SerializeField] private float maxManaMul = 1f;
	//[SerializeField] private float manaRechargeMul = 1f;
	//[SerializeField] private float timeToCastMul = 1f;

	[SerializeField] private float currentMana;
	[SerializeField] private float maxMana = 10f;
	[SerializeField] private float manaRecharageRate = 2f;
	[SerializeField] private float timeToCast = 1f;
	private float tempDmgBoost = 0f;
	private float tempSoulBoost = 0f;

	private float current_difficulty = 1f;

	[SerializeField] private float soulBalance = 0f;

	private bool dead = false;

	private ThirdPersonScript controller;
	public Transform spawnPoint;
	public Canvas shop; 
    private void OnGUI()
    {
		GUI.Box(new Rect(Screen.width - 130, 15, 100, 80), "Heatlh: " + health.ToString() + "\n Mana: " + currentMana.ToString() + "\n Souls: " + soulBalance.ToString() + "\n Difficulty: " + current_difficulty.ToString());
    }
	//Add powerups
	public void GiveTmpHealthBoost(float boost) => health += boost;
	public void GiveTmpDmgBoost(float boost) => tempDmgBoost += boost;
	public void GiveTmpSoulBoost(float boost) => tempSoulBoost += boost;

	//SOUL HANLDING
	public void SetDifficulty(float mod) => current_difficulty = mod;
	public void AddSouls(float souls) => soulBalance += souls;
	public void RemoveSouls(float souls) => soulBalance -= souls;
	public float Souls => soulBalance;
	//Retirm soul multiplier with any temp pickup bonuses
	public float SoulMul => soulMul + tempSoulBoost;
	public void AddSoulMul(float mul) => soulMul += mul;
	// HEALTH HANDLING
	public float MaxHealth => maxHealth;
	public void AddHealth(float moreHealth) => maxHealth += moreHealth;
	public float Health => health;

	public bool isDead => dead; 
    public void SetCurrentMana(float mana) => currentMana = mana;
	public float GetCurrentMana => currentMana;

    public float GetMaxMana => maxMana;
	public float AddMaxMana(float mul) => maxMana += mul;

	public float GetManaRechargeRate => manaRecharageRate;
	public float AddManaRate(float mul) => manaRecharageRate += mul;

	public float GetTimeToCast => timeToCast;
	public float DmgMul => dmgMulti + tempDmgBoost;
	public void AddDmgMul(float mul) => dmgMulti += mul;

    
    void Start()
    {
		controller = GetComponent<ThirdPersonScript>();
		health = maxHealth;
    }

	public void ApplyDamage(float damage){
		health -= damage;
		Debug.Log("Ouch! " + health);
        if (health <= 0){
			health = 0;
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
		GetComponent<CharacterController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		shop.enabled = true;
	}	
	public void Alive()
    {
		shop.enabled = false;
		controller.MoveTo(spawnPoint.position);
		GetComponent<CharacterController>().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		health = maxHealth;
		currentMana = maxMana;
		// Reset powerups
		tempDmgBoost = 0f;
		tempSoulBoost = 0f;

		dead = false;
	}
}
