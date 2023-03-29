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

	private PlayerController playerController;
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

	public void SetEnemies(GameObject[] e)
    {
		playerController.Enemies = e;
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
    
	IEnumerator  Die(){
		dead = true;
		print("Dead!");
		HideCharacter();
		yield return new WaitForSeconds(10);
		print("Alive!");
		//playerController.Respawn();
		ShowCharacter();
		health = maxHealth;
		dead = false;
	}
	
	void HideCharacter(){	
	
		playerController.IsControllable = false;
		
	}
	
	
	
	void ShowCharacter(){


		playerController.IsControllable = true;
	}
	
}
