using UnityEngine;
using System;

public class AIController : MonoBehaviour {

	public float fov = 160.0f;
	private float rotationSpeed = 5.0f;

	private float attackSpeed = 1.2f;
	private float gravity = 50.0f;
	public float attackValue = 20.0f;

	private float attackDistance = 1.5f;
	private float sightDistance = 9.0f;
	private float walkDistance = 2.5f;

	private GameObject player;

	private CharacterController controller;
	private PlayerStatus	playerStatus;
	private Transform		target;
	private Vector3			moveDirection = new Vector3(0,0,0);
	private State			currentState;

	private Animator animator;

	private bool isControllable = true;
	private bool isDead = false;
	private bool deathStarted = false;

	private bool hasAttacked = false;
	private float attackTimer = 0f;

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}
	void ResetAnimationTriggers()
    {
		foreach (var trigger in animator.parameters)
        {
			if(trigger.type == AnimatorControllerParameterType.Bool)
            {
				animator.SetBool(trigger.name, false);
            }
        }
    }
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
		animator = GetComponent<Animator>();
		target = player.transform;
		playerStatus = player.GetComponent<PlayerStatus>();
		ChangeState(new StateIdle());
	}
	
	public void ChangeState(State newState){
		currentState = newState;
		//Set all flags to False for animation
		ResetAnimationTriggers();
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
		animator.SetBool("isRunning", true);

		Vector3 direction = (target.position - controller.transform.position).normalized;

		moveDirection = direction * 3f;
		RotateTowards(target.position, false);
	}
	public void Walk()
    {
		animator.SetBool("isWalking", true);
		Vector3 direction = (target.position - controller.transform.position).normalized;
		moveDirection = direction * 1.8f;
		RotateTowards(target.position, false);
	}
	public void Attack()
    {
		moveDirection = new Vector3(0, 0, 0);
		RotateTowards(target.position, false);
		attackTimer += Time.deltaTime;
		// If animation is not playing
		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
			hasAttacked = false;
            animator.SetBool("isAttacking", true);
			attackTimer = 0f;
		}
		if(attackTimer >= attackSpeed && !hasAttacked)
        {
			playerStatus.ApplyDamage(attackValue);
			hasAttacked = true;
			animator.SetBool("isAttacking", false);
        }
	}
	public void BeDead(){
		if (deathStarted)
		{
			controller.enabled = false;
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
			{
				Destroy(this.gameObject);
			}
		}
		if (!deathStarted)
        {
			animator.SetTrigger("Dead");
			deathStarted = true;
			this.isControllable = false;
		}
	}
	public void BeIdle(){
		moveDirection = new Vector3(0,0,0);
	}
	void Update () {
		currentState.Execute(this);
		if (!isControllable) return;
		moveDirection.y -= gravity*Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
    }
}