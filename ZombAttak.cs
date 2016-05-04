using UnityEngine;
using System.Collections;

public class ZombAttak : MonoBehaviour {

	public bool m_IsAttacking = false;
	public float z_Dmg = 0.1f;
	public AudioSource biteSound;

	Animator zAnim;

	// Use this for initialization
	void Start () {

		zAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Player")) {

			zAnim.SetBool("isAttacking", true);
			m_IsAttacking = true;
			biteSound.Play ();

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
}