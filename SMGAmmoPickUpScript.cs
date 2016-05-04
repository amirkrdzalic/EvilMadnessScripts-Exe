using UnityEngine;
using System.Collections;

public class SMGAmmoPickUpScript : MonoBehaviour {

	public RayShooting shootingScript;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Player")) {
			//smg ammo
			if (shootingScript.GetComponent<RayShooting>().total_SMGAmmo < 300) {
				shootingScript.GetComponent<RayShooting>().total_SMGAmmo += 60;
				shootingScript.GetComponent<RayShooting>().total_SMGAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_SMGAmmo.ToString ();
				if (shootingScript.GetComponent<RayShooting>().total_SMGAmmo > 300) {
					shootingScript.GetComponent<RayShooting>().total_SMGAmmo = 300;
					shootingScript.GetComponent<RayShooting>().total_SMGAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_SMGAmmo.ToString ();
				}
			}
		}
	}
}
