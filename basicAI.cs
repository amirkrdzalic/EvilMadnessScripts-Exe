using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.Thirdperson
{
	public class basicAI : MonoBehaviour {

		public PlayerMovement playerMovement;
		public NavMeshAgent navAgent;

		public ZombAttak m_ZombAttackScript;
		public ZombieHealth m_ZombHealthScript;
		public enum State
		{
			PATROL,
			CHASE
		}
		public State state;

		//variables for patrolling..
		public GameObject[] waypoints;
		private int waypointInd; // use private int waypointInd = 0; for a hardcoded definite route
		public float patrolSpeed = 0.5f;
		//var for chasing..
		public float chaseSpeed = 1.0f;
		public GameObject target;
		Vector3 m_GroundNormal;
		float m_TurnAmount;
		float m_ForwardAmount;
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;

		void Start () {
			navAgent = GetComponent<NavMeshAgent> ();
			playerMovement = GetComponent<PlayerMovement> ();

			navAgent.updatePosition = true;
			navAgent.updateRotation = false;

			waypoints = GameObject.FindGameObjectsWithTag ("WayPoint");
			waypointInd = Random.Range (0, waypoints.Length);

			state = basicAI.State.PATROL;
			//start FSM ...encapsulate FSM in quotes because its faster and you can start stop etc. 
			StartCoroutine("FSM");
		}

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
				}
				yield return null;
			}
		}
		
		void Patrol()
		{
			navAgent.speed = patrolSpeed;
			if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) >= 2) {
				navAgent.SetDestination (waypoints [waypointInd].transform.position);
				//character move
				Move(navAgent.desiredVelocity);
			} 
			else if (Vector3.Distance (this.transform.position, waypoints [waypointInd].transform.position) <= 2) {
				waypointInd = Random.Range (0, waypoints.Length);
				//waypointInd += 1; use the following for definite route system for AI
				//if (waypointInd >= waypoints.Length) {
				//	waypointInd = 0;
				//}
			} 
			else {
				//idle
				Move(Vector3.zero);
			}
		}

		void Chase()
		{
			navAgent.speed = chaseSpeed;
			navAgent.SetDestination (target.transform.position);
			//character
			Move(navAgent.desiredVelocity);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals ("Player")) {
				state = basicAI.State.CHASE;
				target = other.gameObject;
			}
		}


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

		void ApplyExtraTurnRotation()
		{
			//turns faster if needed for smoother animations etc.
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
	}
}