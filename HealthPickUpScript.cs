using UnityEngine;
using System.Collections;

public class HealthPickUpScript : MonoBehaviour {

	public PlayerHealth playerH_script;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Player")) {
			//smg ammo
			if (playerH_script.GetComponent<PlayerHealth>().currentHealth < 100) {
				playerH_script.GetComponent<PlayerHealth>().currentHealth += 40;
				playerH_script.GetComponent<PlayerHealth> ().HandleHealth ();
				//shootingScript.GetComponent<RayShooting>().total_SMGAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_SMGAmmo.ToString ();
				if (playerH_script.GetComponent<PlayerHealth>().currentHealth > playerH_script.GetComponent<PlayerHealth>().maxHealth) {
					playerH_script.GetComponent<PlayerHealth> ().currentHealth = playerH_script.GetComponent<PlayerHealth>().maxHealth;
					playerH_script.GetComponent<PlayerHealth> ().HandleHealth ();
					//shootingScript.GetComponent<RayShooting>().total_SMGAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_SMGAmmo.ToString ();
				}
			}
		}
	}
}
