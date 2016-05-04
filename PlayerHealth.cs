using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	
	public bool playerGot_hit = false;

	public GameObject m_zombie;
	CapsuleCollider player_collider;

	Animator playerAnim;

	//health bar stuff
	public Image VisualHealth;
	public RectTransform healthTransform;
	private float cachedY;
	private float minXvalue;
	private float maxXvalue;
	public float currentHealth;

	private float CurrentHealth {
		get { return currentHealth;}
		set { 
				currentHealth = value;
				HandleHealth();
			}
	}
	public int maxHealth;
	public float coolDown;
	private bool onCD = false;


	void Start ()
	{
		//health bar stuff
		cachedY = healthTransform.position.y;
		maxXvalue = healthTransform.position.x;
		minXvalue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;

		player_collider = gameObject.GetComponent<CapsuleCollider> ();
		playerAnim = gameObject.GetComponent<Animator> ();
	}
	

	//void Update () {
	
	//}

	public void ApplyDamageOnPlayer()
	{
		//&& !onCD && currentHealth > 0
		if (playerGot_hit == true){

			StartCoroutine(CoolDownDMG());
			CurrentHealth -= m_zombie.GetComponent<ZombAttak>().z_Dmg;

			if (CurrentHealth <= 0.0f)
			{
				playerDead();
				player_collider.enabled = false;
				Application.LoadLevel (2);
			}
		}
	}

	public void playerDead()
	{
		//PLACE ANIMATION DEATH STUFF
		playerAnim.SetBool ("PlayerDead", true);
		Destroy (GameObject.Find ("Laser"));
		Destroy (GameObject.Find ("Flash Light"));
		Destroy (GameObject.Find ("DownwardLight"));

	}

	IEnumerator CoolDownDMG()
	{
		onCD = true;
		yield return new WaitForSeconds (coolDown);
		onCD = false;
	}

	public void HandleHealth()
	{
		//text for health stuff here if u want, i dont have any so none.

		//moving the bar to simulate lose of health stuff now
		float currentXvalue = MapValues (currentHealth, 0, maxHealth, minXvalue, maxXvalue);

		healthTransform.position = new Vector3 (currentXvalue, cachedY);

		if (currentHealth > maxHealth / 2) {
			VisualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth/2, maxHealth, 255, 0), 255, 0, 255);
		} 
		else {
			VisualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);
		}
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}