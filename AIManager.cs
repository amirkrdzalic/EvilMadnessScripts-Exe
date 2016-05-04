using UnityEngine;
using System.Collections;

public class AIManager : MonoBehaviour {
	//other SCRIPTS
	public GameObject m_Player;
	public PlayerMovement playerMovement;
	public NavMeshAgent navAgent;
	//public ZombAttak m_ZombAttackScript;
	public ZombieHealth m_ZombHealthScript;
	public Rigidbody rbObject;
	public ZombAttak zAttackScript;

	Animator zAnim;

	public enum State
	{
		WANDER,
		EVADE,
		FLOCKING,
		PATROL,
		CHASE,
	}
	public State state;

	//variables for flocking...
	public bool turning = false;
	public bool isFlocking;
	public float flockSpeed = 0.3f;
	public float rotationSpeed= 4.0f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 4.0f;

	//variables for attack()...which is done through collision
	public bool m_IsAttacking = false;
	public float z_Dmg = 0.05f;

	//variables for wander()....
	public float WanderWalkSpeed = 5f;
	public float directionChangeInterval = 6.0f;
	public float maxHeadingChange = 30f;
	public float directionWander;
	Vector3 targetRotation;
	public bool isWandering = false;

	//variables for patrolling()...
	public bool isPatrolling;
	public GameObject[] waypoints;
	private int waypointInd; // use private int waypointInd = 0; for a hardcoded definite route
	public float patrolSpeed = 1.5f;
	public string tag = "";

	//var for chasing()...
	public bool isChasing = false;
	public float chaseSpeed = 1.0f;
	public GameObject target;
	Vector3 m_GroundNormal;
	float m_TurnAmount;
	float m_ForwardAmount;
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;

	void Awake()
	{
		//random rotation for wandering
		if (isWandering == true && m_ZombHealthScript.GetComponent<ZombieHealth> ().z_Health > 0) {
			directionWander = Random.Range (0, 360);
			transform.eulerAngles = new Vector3 (0, directionWander, 0);
			StartCoroutine (WanderRepeat ());
		}
	}

	void Start () {
		flockSpeed = Random.Range (0.5f, 1);
		target = GameObject.Find ("SoldierPlayer").GetComponent<GameObject> ();

		zAnim = gameObject.GetComponent<Animator>();
		rbObject = gameObject.GetComponent<Rigidbody> ();
		navAgent = GetComponent<NavMeshAgent> ();
		playerMovement = GetComponent<PlayerMovement> ();
		//be able to update position but not rotation
		navAgent.updatePosition = true;
		navAgent.updateRotation = false;
		//finds the waypoints gameobjects
		if (isPatrolling == true) {
			waypoints = GameObject.FindGameObjectsWithTag (tag);
			waypointInd = Random.Range (0, waypoints.Length);
		}
		//starts as patrol
		state = AIManager.State.PATROL;
		//start FSM ...encapsulate FSM in quotes because its faster and you can start stop etc. 
		StartCoroutine("FSM");
	}

	//fixedUpdate, was giving issues.
	void Update()
	{
		if (isWandering == true && m_ZombHealthScript.GetComponent<ZombieHealth> ().z_Health > 0) {
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, targetRotation, Time.deltaTime * maxHeadingChange);
			var forwards = transform.TransformDirection (new Vector3(0, 0, 1) * WanderWalkSpeed);
			rbObject.AddForce (forwards * WanderWalkSpeed);
		}
		if (isFlocking == true && m_ZombHealthScript.GetComponent<ZombieHealth>().z_Health > 0) {
			//transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, targetRotation, Time.deltaTime * maxHeadingChange);
			Flock ();
		}
	}

	//state machine continuous 
	IEnumerator FSM()
	{
		while (m_ZombHealthScript.GetComponent<ZombieHealth> ().z_Health > 0) {
			switch(state)
			{
			case State.PATROL:
				Patrol ();
				break;
			case State.CHASE:
				Chase ();
				break;
			case State.WANDER:
				Wander ();
				break;
			case State.FLOCKING:
				Flock ();
				break;
			}
			yield return null;
		}
	}
	IEnumerator WanderRepeat()
	{
		while (isWandering == true && m_ZombHealthScript.GetComponent<ZombieHealth> ().z_Health > 0) {
			Wander ();
			//changes direction every second
			yield return new WaitForSeconds (directionChangeInterval);
		}
	}

	//randomy picks a gameObject created WAYPOINT
	//inside the game and turns and walks towards it
	void Patrol()
	{
		if (isPatrolling == true) {
			navAgent.speed = patrolSpeed;
			if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) >= 2) {
				navAgent.SetDestination (waypoints [waypointInd].transform.position);
				//character move
				Move (navAgent.desiredVelocity);
			} else if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) <= 2) {
				waypointInd = Random.Range (0, waypoints.Length);
				//waypointInd += 1; use the following for definite route system for AI to pick waypoints
				//if (waypointInd >= waypoints.Length) {
				//	waypointInd = 0;
				//}
			} else {
				//idle
				Move (Vector3.zero);
			}
		}
	}

	//set the speed and target for the gameObject to chase
	void Chase()
	{
		navAgent.speed = chaseSpeed;
		navAgent.SetDestination (target.transform.position);
		//character
		Move(navAgent.desiredVelocity);
	}

	//to move the object towards the gameObject WAYPOINT for patrol
	public void Move(Vector3 move)
	{
		//converts world to local and help with forward movement etc.
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

		ApplyExtraTurnRotation();
	}

	//to help the gameObject turn towards the WAYPOINTS
	void ApplyExtraTurnRotation()
	{
		//turns faster if needed for smoother animations etc.
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}

	//wander
	void Wander()
	{
		//picks either a direction to rotate to wander in
		//but has a max of 30 degrees left or right to turn too as a choice
		var newDirection1 = Mathf.Clamp(directionWander - maxHeadingChange, 0, 360);
		var newDirection2 = Mathf.Clamp (directionWander + maxHeadingChange, 0, 360);
		directionWander = Random.Range (newDirection1, newDirection2);
		targetRotation = new Vector3 (0, directionWander, 0);
	}

	//trigger for the collision box,
	//if there is a collion then go to CHASE mode
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals ("Player"))
		{
			isFlocking = false;
			isChasing = true;
			state = AIManager.State.CHASE;
			target = other.gameObject;
		}
	}

	//ATTACKING DONE THROUGH COLLISIONS----------------------
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Player")) {

			zAnim.SetBool("isAttacking", true);
			m_IsAttacking = true;

			GameObject Player = other.collider.gameObject;
			Player.GetComponent<PlayerHealth>().playerGot_hit = true;
			Player.GetComponent<PlayerHealth>().ApplyDamageOnPlayer();
		}
	}
	void OnCollisionStay(Collision other)
	{
		if (other.gameObject.CompareTag("Player")){
			GameObject Player = other.collider.gameObject;
			Player.GetComponent<PlayerHealth>().ApplyDamageOnPlayer();
		}
	}
	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			zAnim.SetBool("isAttacking", false);
			m_IsAttacking = false;
		}
	}

	void Flock()
	{
		if (isFlocking == true && m_ZombHealthScript.GetComponent<ZombieHealth>().z_Health > 0)
		{
			if (Vector3.Distance (transform.position, Vector3.zero) >= globalFlock.size) {
				turning = true;
			} else {
				turning = false;
			}
			if (turning) {
				Vector3 direction = Vector3.zero - transform.position;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position), rotationSpeed * Time.deltaTime);
				flockSpeed = Random.Range (0.5f, 1);
			}
			//change the 5 to higher to decrease amount of flocking
			//applys the rules to the flock group to prevent drifting etc.
			else {
				if (Random.Range (0, 5) < 1) {
					ApplyRulesForFlock ();
				}
			}
			transform.LookAt (m_Player.transform);
			if (!zAttackScript.GetComponent<ZombAttak> ().m_IsAttacking)
			{
				transform.position += transform.forward * flockSpeed * Time.deltaTime;
			}
		}
	}

	void ApplyRulesForFlock()
	{
		if (isFlocking == true) {
			GameObject[] gos;
			gos = globalFlock.allZombies;
			Vector3 vCentre = Vector3.zero;
			Vector3 vAvoid = Vector3.zero;
			float gSpeed = 0.2f;
			Vector3 goalpos = globalFlock.goalpos;

			float dis;
			int groupsize = 0;

			foreach (GameObject go in gos) {
				if (go != this.gameObject) {
					dis = Vector3.Distance (go.transform.position, this.transform.position);
					//if within this distance then ur a neighbour flocker
					if (dis <= neighbourDistance) {
						vCentre += go.transform.position;
						groupsize++;
						//if to close back off
						if (dis < 1.0f) {
							vAvoid = vAvoid + (this.transform.position - go.transform.position);
							//find avrg speed
							AIManager anotherFlock = go.GetComponent<AIManager> ();
							//add to total speed to find avrg speed
							gSpeed = gSpeed + anotherFlock.flockSpeed;
						}
					}
				}

				if (groupsize > 0) {
					//average centre
					vCentre = vCentre / groupsize + (goalpos - this.transform.position);
					//average speed
					flockSpeed = gSpeed; // / groupsize
					//direction without hitting
					Vector3 direction = (vCentre + vAvoid) - transform.position;
					//if (direction != Vector3.zero)
					//{
						//change directions rotation with SLERP no snapping
						//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
					//}
				}
			}
		}
	}
}

