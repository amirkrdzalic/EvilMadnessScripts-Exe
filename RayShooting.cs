using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RayShooting : MonoBehaviour{

	//timer for Shotgun Cock back
	Timer shotGunCockBack_Timer = new Timer();

	//audio
	public AudioSource Rifle_Audio_Shot;
	public AudioSource SMG_Audio_Shot;
	public AudioSource Shotgun_Audio_Shot;
	public AudioSource GLauncher_Audio_Shot;
	public AudioSource GLauncher_Audio_Explosion;

	//player dmg
	public float RifleDMG;
	public float ShotGunDMG;
	public float SmgDMG;
	public float GLauncherDMG;

	//player
	public GameObject m_Player;
	PlayerMovement m_MoveScript;
	PlayerHealth m_HealthScript;

	//laser stuff
	Transform Effect;
	public const float maxDistanceLaser = 20.0f;
	Vector3 origin;
	//laser
	public LineRenderer m_Laser;


	//shotgun  and code for collider blast
	public GameObject m_ShotGunBlast;
	public float shotgun_SpreadFactor = 0.1f;
	//guns by look.
	public int inc = 0; //weapon cycling incrementer
	public GameObject rifle;
	public GameObject smg;
	public GameObject shotgun;
	public GameObject gLauncher;
	//grenade launcher bullet
	public GameObject gLauncher_Bullet;
	public GameObject gLauncher_Bullet_Spawn;

	//max RANGE for each GUN---
	private float Rifle_Range = 40.0f;
	private float SMG_Range = 30.0f;
	private float GLauncher_Range = 40.0f;
	private float Shotgun_Range = 10.0f;

	//Fire rate
	private float RIFLE_nextFire = 0.0f;
	private float RIFLE_fireRate = 0.3f;
	private float SMG_nextFire = 0.0f;
	private float SMG_fireRate = 0.1f;


	//ammo COUNT----
	public bool isReloading = false;
	private bool outOfSMGAmmo = true;
	private bool outOfShotgunAmmo = false;
	private bool outOfGLAmmo = true;
	public bool totalAmmoSubtraction = false;

	public int current_rifleAmmo;
	public int current_SMGAmmo;
	public int current_ShotgunAmmo;
	public int current_GLauncherAmmo;
	//total Ammo count ------
	public int total_SMGAmmo;
	public int total_ShotgunAmmo;
	public int total_GLauncherAmmo;

	//AMMO ICONS----111---
	public WeaponSwitchingIcons m_weaponIconScript;
	public Text current_RifleAmmo_text;
	public Text current_ShotgunAmmo_text;
	public Text current_SMGAmmo_text;
	public Text current_GLauncherAmmo_text;
	//total
	public Text total_ShotgunAmmo_text;
	public Text total_SMGAmmo_text;
	public Text total_GLauncherAmmo_text;

	void Start () {
		//text GUI
		current_RifleAmmo_text.text = current_rifleAmmo.ToString();
		current_ShotgunAmmo_text.text = current_ShotgunAmmo.ToString ();
		current_SMGAmmo_text.text = current_SMGAmmo.ToString ();
		current_GLauncherAmmo_text.text = current_GLauncherAmmo.ToString ();

		total_ShotgunAmmo_text.text = total_ShotgunAmmo.ToString ();
		total_SMGAmmo_text.text = total_SMGAmmo.ToString ();
		total_GLauncherAmmo_text.text = total_GLauncherAmmo.ToString ();

		//audio
		AudioSource[] audios = GetComponents<AudioSource>();
		Rifle_Audio_Shot = audios [0];
		SMG_Audio_Shot = audios [1];
		Shotgun_Audio_Shot = audios [2];
		GLauncher_Audio_Shot = audios [3];
		GLauncher_Audio_Explosion = audios [4];

		m_MoveScript = (PlayerMovement)m_Player.GetComponent<PlayerMovement>();
		m_HealthScript = (PlayerHealth)m_Player.GetComponent<PlayerHealth> ();
	}
	

	void Update () {

		if (current_rifleAmmo == 0 || current_SMGAmmo == 0 || current_ShotgunAmmo == 0 || current_GLauncherAmmo == 0) {
			isReloading = true;
			if (isReloading == true) {
				StartCoroutine ("Reloading");
			}
			if (total_SMGAmmo == 0 && current_SMGAmmo == 0) {
				outOfSMGAmmo = true;
			}
			else if (total_ShotgunAmmo == 0 && current_ShotgunAmmo == 0) {
				outOfShotgunAmmo = true;
			}
			else if (total_GLauncherAmmo == 0 && current_GLauncherAmmo == 0) {
				outOfGLAmmo = true;
			}

			if (totalAmmoSubtraction == false) {
				
				total_SMGAmmo -= current_SMGAmmo;
				total_ShotgunAmmo -= current_ShotgunAmmo;
				total_GLauncherAmmo -= current_GLauncherAmmo;

				total_ShotgunAmmo_text.text = total_ShotgunAmmo.ToString ();
				total_SMGAmmo_text.text = total_SMGAmmo.ToString ();
				total_GLauncherAmmo_text.text = total_GLauncherAmmo.ToString ();
				totalAmmoSubtraction = true;
			}
		}

		//if hes alive then.... do everything
		if (m_HealthScript.currentHealth > 0) {

			if (shotGunCockBack_Timer.IsTimePassed (ShotGunCollision.SHOTGUN_DELAY)) {
				shotGunCockBack_Timer.StopTimer ();
				m_ShotGunBlast.gameObject.GetComponent<ShotGunCollision> ().SetAllCollidersStatus (false);
				m_ShotGunBlast.gameObject.GetComponent<BoxCollider> ().enabled = false;

			}

			//timer
			shotGunCockBack_Timer.Update ();
			//------------------------------------------------------------------


			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			RaycastHit hit2;

			Vector3 lookpos = Vector3.zero;

			//laser stuff--------------------------------------------------------------
			if (Physics.Raycast (ray, out hit, 360)) {				
				lookpos = hit.point;	
			}
		
			lookpos.y = m_MoveScript.m_RiflePosition.position.y;

			if (Physics.Raycast (m_MoveScript.m_RiflePosition.position, (lookpos - m_MoveScript.m_RiflePosition.position), out hit, Mathf.Infinity)) {

				if (hit.distance > Rifle_Range && rifle.activeSelf) {
					hit.distance = Rifle_Range;
				}
				if (hit.distance > SMG_Range && smg.activeSelf) {
					hit.distance = SMG_Range;
				}
				if (hit.distance > GLauncher_Range && gLauncher.activeSelf) {
					hit.distance = GLauncher_Range;
				}
				if (hit.distance > Shotgun_Range && shotgun.activeSelf) {
					hit.distance = Shotgun_Range;
				}

				m_Laser.SetPosition (1, new Vector3 (0, 0, hit.distance));
			}
			//laser stuff ends------------------------------------------------------------


			//CALLS THE WEAPON SWAP
			if (Input.GetButtonDown ("Swap")) {
				inc++;
				SwapWeapons ();

			}
			//////////////////////////

			//SHOOTING
			if (Input.GetMouseButtonDown (0) && isReloading == false) {
				//RIFLE-------------------------------------------------------------------------------------------------------------------
				if (rifle.activeSelf && Time.time > RIFLE_nextFire) {
					RIFLE_nextFire = Time.time + RIFLE_fireRate;
					//aim with mouse....
					if (Physics.Raycast (ray, out hit, 360)) {				
						lookpos = hit.point;
					}
					lookpos.y = m_MoveScript.m_RiflePosition.position.y;

					//ammo----
					current_rifleAmmo--;
					current_RifleAmmo_text.text = current_rifleAmmo.ToString ();
					//audio
					Rifle_Audio_Shot.Play ();

					//raycast for bullets....starts here
					if (Physics.Raycast (m_MoveScript.m_RiflePosition.position, (lookpos - m_MoveScript.m_RiflePosition.position), out hit, Rifle_Range)) {
						Debug.Log (hit.collider);
						if (hit.collider.gameObject.tag.Equals ("Zombie")) {
							GameObject Zombie = hit.collider.gameObject;
							Zombie.GetComponent<ZombieHealth> ().z_gotHit = true;
							Zombie.GetComponent<ZombieHealth> ().ApplyDamageOnZombie ();
						}
					}
					//ends here...
				}
			//SHOTGUN-----------------------------------------------------------------------------------------------------------------
				else if (shotgun.activeSelf && outOfShotgunAmmo == false) {
					if (shotGunCockBack_Timer.getIsPaused ()) {
						Debug.Log ("Shot Shotgun");

						if (Physics.Raycast (ray, out hit, 360)) {				
							lookpos = hit.point;
						}
						lookpos.y = m_MoveScript.m_ShotgunPosition.position.y;
						//ammo----
						current_ShotgunAmmo--;
						current_ShotgunAmmo_text.text = current_ShotgunAmmo.ToString ();
						//audio
						Shotgun_Audio_Shot.Play ();
						//raycast for bullets....starts here

						for (int i = 0; i <= 10; i++) {
							Vector3 direction = transform.forward;
							direction.x += Random.Range (-shotgun_SpreadFactor, shotgun_SpreadFactor);
							direction.y += Random.Range (-shotgun_SpreadFactor, shotgun_SpreadFactor);
							direction.z += Random.Range (-shotgun_SpreadFactor, shotgun_SpreadFactor);
							if (Physics.Raycast (m_MoveScript.m_ShotgunPosition.position, (lookpos - m_MoveScript.m_RiflePosition.position), out hit, 15.0f))
							{
								Debug.Log (hit.collider.name);
								if (hit.collider.gameObject.tag.Equals ("Zombie")) {
									GameObject Zombie = hit.collider.gameObject;
									Zombie.GetComponent<ZombieHealth> ().z_gotHit = true;
									Zombie.GetComponent<ZombieHealth> ().ApplyDamageOnZombie ();
								}
							}
						}

						//m_ShotGunBlast.gameObject.GetComponent<ShotGunCollision> ().SetAllCollidersStatus (true);
						//m_ShotGunBlast.gameObject.GetComponent<BoxCollider>().enabled = true;

						shotGunCockBack_Timer.RestartTimer ();
					}
				}
			//GRENADE LAUNCHER----------------------------------------------------------------------------------------------------------
				else if (gLauncher.activeSelf) {
					//spawns grenade but doesnt give the force.
					Instantiate (gLauncher_Bullet, gLauncher_Bullet_Spawn.transform.position, gLauncher_Bullet_Spawn.transform.rotation);
					//audio
					GLauncher_Audio_Shot.Play ();
					//ammo
					current_GLauncherAmmo--;
					current_GLauncherAmmo_text.text = current_GLauncherAmmo.ToString ();
				}
			}

			//SMG--------------FOR HOLDING AUTOMATIC GUN------------------------------------------------------------------------------------
			if (Input.GetMouseButton (0) && Time.time > SMG_nextFire && smg.activeSelf && isReloading == false) {
				if (Physics.Raycast (ray, out hit, 360)) {				
					lookpos = hit.point;
				}
				lookpos.y = m_MoveScript.m_SMGPosition.position.y;

				//fire rate
				SMG_nextFire = Time.time + SMG_fireRate;
				//ammo---
				current_SMGAmmo--;
				current_SMGAmmo_text.text = current_SMGAmmo.ToString ();
				//audio
				SMG_Audio_Shot.Play ();

				if (Physics.Raycast (m_MoveScript.m_SMGPosition.position, (lookpos - m_MoveScript.m_SMGPosition.position), out hit, SMG_Range)) {
					if (hit.collider.gameObject.tag.Equals ("Zombie")) {
						GameObject Zombie = hit.collider.gameObject;
						Zombie.GetComponent<ZombieHealth> ().z_gotHit = true;
						Zombie.GetComponent<ZombieHealth> ().ApplyDamageOnZombie ();
					}
				}
			}
		}
	}

	//WEAPON SWAPSSSSS-------------------------------------------------------------------------------------------------------------
	void SwapWeapons()
	{
		//RIFLE IS DEFAULT
		
		//shotgun
		if (inc == 1)
		{
			rifle.SetActive(false);
			shotgun.SetActive(true);
		}
		//smg
		if (inc == 2)
		{
			shotgun.SetActive(false);
			smg.SetActive(true);
		}
		//glauncher
		if (inc == 3)
		{
			smg.SetActive(false);
			gLauncher.SetActive(true);
		}
		//back to rifle
		if (inc == 4)
		{
			gLauncher.SetActive(false);
			rifle.SetActive(true);
		}
		//back to rifle
		if (inc == 4)
		{
			inc = 0;
		}
	}

	IEnumerator Reloading()
	{
		yield return new WaitForSeconds (2.0f);
		isReloading = false;

		current_SMGAmmo = 60;
		total_SMGAmmo -= current_SMGAmmo;
		current_rifleAmmo = 30;
		current_ShotgunAmmo = 7;
		total_ShotgunAmmo -= current_ShotgunAmmo;
		current_GLauncherAmmo = 4;
		total_GLauncherAmmo -= current_GLauncherAmmo;

		current_RifleAmmo_text.text = current_rifleAmmo.ToString();
		current_ShotgunAmmo_text.text = current_ShotgunAmmo.ToString ();
		current_SMGAmmo_text.text = current_SMGAmmo.ToString ();
		current_GLauncherAmmo_text.text = current_GLauncherAmmo.ToString ();
	}
}