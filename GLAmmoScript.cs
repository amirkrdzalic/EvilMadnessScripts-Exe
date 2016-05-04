using UnityEngine;
using System.Collections;

public class GLAmmoScript : MonoBehaviour {

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
			if (shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo < 12) {
				shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo += 4;
				shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo.ToString ();
				if (shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo > 12) {
					shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo = 12;
					shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo_text.text = shootingScript.GetComponent<RayShooting>().total_GLauncherAmmo.ToString ();
				}
			}
		}
	}
}
