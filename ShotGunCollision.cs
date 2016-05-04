using UnityEngine;
using System.Collections;

public class ShotGunCollision : MonoBehaviour {

	public static BoxCollider shotgunCollider;

	Timer m_DisableTimer = new Timer();
	public static long SHOTGUN_DELAY = 200;

	public Rigidbody player;

	public bool shotGunCollision_hasShot = false;


	// Use this for initialization
	void OnEnable ()
	{
		Debug.Log ("START");
		m_DisableTimer.StartTimer ();

		gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 1.53f, player.transform.position.z + 1.3f);// + player.transform.forward;

	}
	void Start()
	{
		shotgunCollider = gameObject.GetComponent<BoxCollider> ();
		//doesnt do anything...
		shotGunCollision_hasShot = false;

	}

	void Update () 
	{
		m_DisableTimer.Update ();
		//gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 1.53f, player.transform.position.z + 6.3f);// + player.transform.forward;


		if (m_DisableTimer.IsTimePassed (SHOTGUN_DELAY)) 
		{
			m_DisableTimer.StopTimer();

			SetAllCollidersStatus (false);
			shotgunCollider.enabled = false;

			//gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider otherCollider)
	{
		Debug.Log ("" + otherCollider.gameObject.name);

		if (m_DisableTimer.IsTimePassed (SHOTGUN_DELAY)) 
		{
			m_DisableTimer.StopTimer();

			SetAllCollidersStatus (false);
			shotgunCollider.enabled = false;
		}

		m_DisableTimer.StartTimer ();

		if (otherCollider.gameObject.tag.Equals("Zombie"))
		{	 
			GameObject Zombie = otherCollider.gameObject;
			if(!Zombie.GetComponent<ZombieHealth>().z_gotHit)
			{
				Debug.Log("it hit inside ShotGun");
				Zombie.GetComponent<ZombieHealth>().z_gotHit = true;
				Zombie.GetComponent<ZombieHealth>().ApplyDamageOnZombie();
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("" + collision.gameObject.name);
		
		if (m_DisableTimer.IsTimePassed (SHOTGUN_DELAY)) 
		{
			m_DisableTimer.StopTimer();
			shotgunCollider.enabled = false;
		}
		
		m_DisableTimer.StartTimer ();
		
		if (collision.gameObject.tag.Equals("Zombie"))
		{	 
			GameObject Zombie = collision.gameObject;
			if(!Zombie.GetComponent<ZombieHealth>().z_gotHit)
			{
				Debug.Log("it hit inside ShotGun");
				Zombie.GetComponent<ZombieHealth>().z_gotHit = true;
				Zombie.GetComponent<ZombieHealth>().ApplyDamageOnZombie();
			}
		}
	}

	public void SetAllCollidersStatus(bool active)
	{
		foreach (Collider col in GetComponentsInChildren<Collider>()) {
			col.enabled = active;
		}
	}
}