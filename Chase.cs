using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour
{
	public float Speed = 5f;
	public float withInRange = 5f;
	public NavMeshAgent zombieNav;

	ZombAttak m_ZombAttackScript;
	ZombieHealth m_ZombHealthScript;

	//dont touch!
	//cant find prefab Soldier so place GameObject Player,,, only way it works
	private GameObject m_Player;

	// Use this for initialization
	void Start ()
	{
		GameObject envir = GameObject.Find ("Environment");
		m_ZombHealthScript = gameObject.GetComponent<ZombieHealth> ();
		m_ZombAttackScript = gameObject.GetComponent<ZombAttak> ();
		zombieNav = gameObject.GetComponent<NavMeshAgent> ();
		m_Player = GameObject.Find ("SoldierPlayer");

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_ZombHealthScript.z_Health > 0) 
		{
			if (!m_ZombAttackScript.m_IsAttacking)
			{
				transform.LookAt(m_Player.transform);
				if (Vector3.Distance(transform.position, m_Player.transform.position) <= withInRange)
				{
					transform.position += transform.forward*Speed*Time.deltaTime;
				}
			}
		}
	}
}