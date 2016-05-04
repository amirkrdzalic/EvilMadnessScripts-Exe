using UnityEngine;
using System.Collections;

public class ShotgunAmmoScript : MonoBehaviour {

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
			if (shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo < 45) {
				shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo += 7;
				shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo.ToString ();
				if (shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo > 45) {
					shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo = 45;
					shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_ShotgunAmmo.ToString ();
				}
			}
		}
	}
}
