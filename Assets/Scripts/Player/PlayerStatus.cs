using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	[SerializeField]
	private float health = 150.0f;
	[SerializeField]
	private float maxHealth = 150.0f;
	[SerializeField]
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
	public Canvas shop; 
    private void OnGUI()
    {
		GUI.Box(new Rect(Screen.width - 100, 5, 100, 50), "Heatlh: " + health.ToString() + "\n Mana: " + currentMana.ToString() + "\n Souls: " + soulBalance.ToString());
    }
	//SOUL HANLDING
	public void AddSouls(float souls) => soulBalance += souls;
	public void RemoveSouls(float souls) => soulBalance -= souls;
	public float GetSouls() => soulBalance;
	// HEALTH HANDLING
	public float MaxHealth => maxHealth;
	public void AddHealth(float moreHealth) => maxHealth += moreHealth;
	public float Health => health;

	public bool isDead => dead; 
    public void SetCurrentMana(float mana) => currentMana = mana;
	public float GetCurrentMana => currentMana;
    public float GetMaxMana => maxMana;
	public float GetManaRechargeRate => manaRecharageRate;
	public float GetTimeToCast => timeToCast;
	public float DmgMul => dmgMulti;

    
    void Start()
    {
		controller = GetComponent<ThirdPersonScript>();
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
		PopulateShop pop = shop.GetComponent<PopulateShop>();

		deathCount += 1;
		dead = true;
		DespawnEnemies();
		// Tele back to spawn point
		GetComponent<CharacterController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		shop.enabled = true;
		// Remove all enemies
		// Spawn all enemies
		// Show shop
		// Release cursor
		// Once shop is closed , lock cursor and hide ui
	}	
	public void Alive()
    {
		shop.enabled = false;
		controller.MoveTo(spawnPoint.position);
		GetComponent<CharacterController>().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		health = maxHealth;
		currentMana = maxMana;
		dead = false;
	}
}
