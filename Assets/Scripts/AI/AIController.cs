using UnityEngine;
using System;

public class AIController : MonoBehaviour {
	enum PowerUpType
	{
		HealthUp,
		DmgUp,
		SoulUp,
		None
	};
	[SerializeField]
	PowerUpType powerType = new PowerUpType();
	enum EnemyType
	{
		Melee,
		Range,
		Boss
	};
	[SerializeField]
	EnemyType type = new EnemyType();

	public float fov = 260.0f;
	[SerializeField]
	private float rotationSpeed = 5.0f;
	[SerializeField]
	private float attackSpeed = 0.8f;
	private float gravity = 50.0f;


	[SerializeField]
	private float attackDistance = 1.5f;
	[SerializeField]
	private float sightDistance = 14.0f;
	[SerializeField]
	private float walkDistance = 2.5f;

	public Transform castPoint;
	public Spell spell;
	public PowerUp powerupPrefab;

	private GameObject player;

	private CharacterController controller;
	private CapsuleCollider capsule;
	private PlayerStatus	playerStatus;
	private Transform		target;
	private Vector3			moveDirection = new Vector3(0,0,0);
	private State			currentState;
	private AIStat stats;

	private Animator animator;

	private bool isControllable = true;
	private bool deathStarted = false;

	private bool hasAttacked = false;

	private float attackTimer = 0f;

	public bool IsDead => stats.IsDead;

 	// My A1 Rotation script
	private void RotateTowards(Vector3 target, bool inverse)
	{
		Quaternion targetRotation;
		// Find the target position relative to the current positon
		Vector3 relative = transform.InverseTransformPoint(target);
		// Find the relative target angle
		float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
		// using the current rotation as a starting point, find target angle
		angle += transform.eulerAngles.y;
		//Convert to Quaternion
		targetRotation = Quaternion.Euler(0f, angle, 0f);
        if (inverse)
		{
			targetRotation = Quaternion.Euler(0f, angle+180f, 0f);

		}
		// Smooth rotation
		// found on the unity forums F
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		controller = GetComponent<CharacterController>();
		controller.detectCollisions = false;
		capsule = GetComponent<CapsuleCollider>();
		animator = GetComponent<Animator>();
		target = player.transform;
		playerStatus = player.GetComponent<PlayerStatus>();
		ChangeState(new StateIdle());
		stats = GetComponent<AIStat>();
	}
	
	public void ChangeState(State newState){
		currentState = newState;
		animator.SetBool("Attacking", false);
	}
	 
	// == Conditions for State Machine == 
	public Boolean ShouldRun()
    {
		float disp = Vector3.Distance(target.position, controller.transform.position);
		if(disp <= walkDistance)
        {
			return false;
        }
		return true;
	}
	public Boolean EnemySeen()
    {
        Vector3 d = target.position - controller.transform.position;
        Vector3 v = controller.transform.forward;

        float angle = Mathf.Acos(Vector3.Dot(d.normalized, v))*Mathf.Rad2Deg;

        if (angle < (fov / 2) && Vector3.Distance(controller.transform.position, target.position) <= sightDistance)
        {
            return true;
        }
		return false;

    }
	public Boolean EnemyInRange()
    {
		if (Vector3.Distance(target.position, controller.transform.position) <= attackDistance)
        {
			return true;
        } 
		return false;
    }
	public void Run()
    {
		animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
		Vector3 direction = (target.position - controller.transform.position).normalized;
		moveDirection = direction * 3f;
		RotateTowards(target.position, false);
	}
	public void Walk()
    {
		animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
		Vector3 direction = (target.position - controller.transform.position).normalized;
		moveDirection = direction * 1.8f;
		RotateTowards(target.position, false);
	}
	public void Attack()
    {
		moveDirection = new Vector3(0, 0, 0);
		RotateTowards(target.position, false);
		attackTimer += Time.deltaTime;

		//if animation is not currently playing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
			animator.SetBool("Attacking", true);
			hasAttacked = false;
			attackTimer = 0f;
		}
		if((attackTimer >= attackSpeed) && !hasAttacked)
        {
			if (type == EnemyType.Range)
			{
				Spell s = Instantiate(spell, castPoint.position, castPoint.rotation);
				//Only damage players prevent friendly fire
				s.SetDmgType(2);
			}
			else if (type == EnemyType.Melee)
            {
				playerStatus.ApplyDamage(stats.Dmg);
			}
			hasAttacked = true;
		}
	}

	public void BeDead(){
		if (deathStarted)
		{
			controller.enabled = false;
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
			{
				PowerUp p = Instantiate(powerupPrefab, castPoint.position, castPoint.rotation);
				p.SetType((int)powerType);
				Destroy(this.gameObject);
			}
		}
		if (!deathStarted)
        {
			animator.SetTrigger("Dead");
			deathStarted = true;
			this.isControllable = false;
			capsule.enabled = false;
		}
	}
	public void BeIdle(){
		moveDirection = new Vector3(0,0,0);
		animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

	}
	void Update () {
		currentState.Execute(this);
		if (!isControllable) return;
		moveDirection.y -= gravity*Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
    }
}