using UnityEngine;
using System;

public class AIController : MonoBehaviour {

	public float fov = 160.0f;
	private float rotationSpeed = 5.0f;

	private float 			attackSpeed = 0.8f;
	private float 			gravity = 50.0f;
	private float			attackValue = 5.0f;

	private float attackDistance = 1.5f;
	private float sightDistance = 12.0f;
	private float walkDistance = 2.5f;

	private CharacterController controller;
	private PlayerStatus	playerStatus;
	private Transform		target;
	private Vector3			moveDirection = new Vector3(0,0,0);
	private State			currentState;
	private Animation		anim;

	private bool			isControllable = true;
	private bool			isDead = false;
	private bool inDanger = false;

	private bool hasAttacked = false;
	private float attackTimer = 0f;

	//This is a hack for legacy animation - we will do this properly later
	private bool			deathStarted = false; 


	public bool 	IsControllable {
		get {return isControllable;}
		set {isControllable = value;}
	}

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}

	public bool InDanger
    {
		get { return inDanger; }
		set { inDanger = value; }
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
		controller = GetComponent< CharacterController>();
		anim = GetComponent<Animation>();
		GameObject tmp = GameObject.FindWithTag("Player");
		if (tmp != null){
			target=tmp.transform;
			playerStatus = tmp.GetComponent< PlayerStatus>();
		}

		ChangeState(new StateIdle());
	}
	
	public void ChangeState(State newState){
		currentState = newState;
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

	// === STATE LOGIC === 
	public void Run()
    {
		Vector3 direction = (target.position - controller.transform.position).normalized;
		anim["run"].speed = 1f;
		if (!anim.IsPlaying("run"))
		{
			anim.CrossFade("run", 0.2f);
		}
		moveDirection = direction * 1.8f;
		RotateTowards(target.position, false);
	}

	public void Walk()
    {
		Vector3 direction = (target.position - controller.transform.position).normalized;
        anim["run"].speed = 0.5f;
        if (!anim.IsPlaying("run"))
        {
			anim.CrossFade("run", 0.2f);
		}
		moveDirection = direction;
		RotateTowards(target.position, false);
	}

	public void RunAway()
    {
		Vector3 direction = (target.position - controller.transform.position).normalized;
		anim["run"].speed = 1f;
		if (!anim.IsPlaying("run"))
		{
			anim.CrossFade("run", 0.2f);
		}
		moveDirection = -direction * 1.8f;

		RotateTowards(target.position, true);

	}

	public void Attack()
    {
		moveDirection = new Vector3(0, 0, 0);
		RotateTowards(target.position, false);

		// We use a timer to ensure that enough of the animation has played to apply damage
		// Prevents too much damage being applied when moving between walk and attack states
		attackTimer += Time.deltaTime;

		if (!anim.IsPlaying("attack"))
        {
			attackTimer = 0f;
			hasAttacked = false;
			anim.CrossFade("attack", 0.1f);

		}
		if (attackTimer >= attackSpeed && !hasAttacked)
        {
			playerStatus.ApplyDamage(attackValue);
			hasAttacked = true;
		}
    }

	public void BeDead(){
		//This is a hack for legacy animation - we will do this properly later
		if (!deathStarted)
		{
			anim.CrossFade("die", 0.1f);
			deathStarted = true;
			CharacterController controller = GetComponent<CharacterController>();
			controller.enabled = false;

		}
		moveDirection = new Vector3(0,0,0);
		if (!anim.isPlaying)
        {
			gameObject.SetActive(false);
			this.IsControllable = false;
		}
	}
	
	public void BeIdle(){
		anim.CrossFade("idle", 0.2f);	
		moveDirection = new Vector3(0,0,0);
	}

	void Update () {
		if (!isControllable)
			return;
		currentState.Execute(this);	
		moveDirection.y -= gravity*Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

    }

	// == Game Object Cleanup == 
	void OnDisable()
	{
		Destroy(gameObject);
        playerStatus.AddHealth(10);
	}

    private void OnDestroy()
    {
		GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
		playerStatus.SetEnemies(e);
		Debug.Log(e.Length);
	}

}