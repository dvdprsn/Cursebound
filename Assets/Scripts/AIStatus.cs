using UnityEngine;
using System.Collections;

public class AIStatus : MonoBehaviour {
	
	private float	health = 100.0f;
    private float maxHealth = 100.0f;

    private bool dead = false;
    private AIController aiController;
	
	void Start(){
		aiController = GetComponent<AIController>();
	}

    public bool isAlive() { return !dead; }

    public void ApplyDamage(float damage){
		Debug.Log("Enemy NPC damage " + damage);
		health -= damage;
		Debug.Log(health / maxHealth);

		if (health <= 0 && !aiController.IsDead)
		{
			dead = true;
			health = 0;
			print("***********Dead!*************");
			aiController.IsDead = true;

		}
		else if ((health / maxHealth) <= 0.1f && isAlive())
		{
			aiController.InDanger = true;
		}
	}
}
