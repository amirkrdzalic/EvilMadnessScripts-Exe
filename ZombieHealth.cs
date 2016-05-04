using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {

	public float z_Health;
	public bool z_gotHit;
	public Chase m_chaseScript;

	//public ZombAttak z_AttackScript;
	public GameObject raycast;
	Animator zAnim;
	public Rigidbody zombieRB;
	CapsuleCollider z_collider;
	Chase z_chase_script;


	// Use this for initialization
	void Start () 
	{
		z_gotHit = false;
		z_chase_script = gameObject.GetComponent<Chase> ();
		zAnim = gameObject.GetComponent<Animator> ();
		z_collider = gameObject.GetComponent<CapsuleCollider> ();
		m_chaseScript = gameObject.GetComponent<Chase> ();
		zombieRB = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ApplyDamageOnZombie()
	{
		if (z_gotHit == true) {

			//RIFLE
			if (GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().rifle.activeSelf)
			{
				z_Health -= GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().RifleDMG;
			}

			//SHOTGUN
			else if (GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().shotgun.activeSelf)
			{
				z_Health -= GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().ShotGunDMG;
				Debug.Log(z_Health);
			}
			//smg
			else if (GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().smg.activeSelf)
			{
				z_Health -= GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().SmgDMG;
				Debug.Log(z_Health);
			}
			//GL
			else if (GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().gLauncher.activeSelf)
			{
				z_Health -= GameObject.Find("Gun-RayCast").GetComponent<RayShooting>().GLauncherDMG;
				Debug.Log(z_Health);
			}

			z_gotHit = false;
			//death
			if (z_Health <= 0)
			{
				Debug.Log("hes dead");
				zDead();
				Destroy (gameObject, 10.0f);
				m_chaseScript.zombieNav.enabled = false;
				z_collider.enabled = false;
				zombieRB.isKinematic = false;
			}
		}
	}

	//ZOMBIE IS DEAD
	public void zDead()
	{
		//gameobjected
		zAnim.SetBool ("isDead", true);
		zAnim.SetBool ("isAttacking", false);
	}
}