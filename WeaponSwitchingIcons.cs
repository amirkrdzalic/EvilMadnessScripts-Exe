using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponSwitchingIcons : MonoBehaviour {

	public RayShooting gunraycast;

	//int currentGun;
	public GameObject[] gunIcons;
	public GameObject[] ammoHolders;
	
	void Start () {
	}
	
	void Update () {
		//call every frame to make changes...
		changeGun ();
	}

	//keeps track of what gun you have selected from RAYSHOOTING SCRIPT, and makes changes accordingly...
	public void changeGun() {

		for(int i = 0; i < gunIcons.Length; i++) {
			if(i == gunraycast.GetComponent<RayShooting>().inc)
				gunIcons[i].GetComponent< CanvasGroup >().alpha = 1;
			else 
				gunIcons[i].GetComponent< CanvasGroup >().alpha = 0.25f;
		}
		//Toggles the ammo holders
		for(int o = 0; o < ammoHolders.Length; o++) {
			if(o == gunraycast.GetComponent<RayShooting>().inc)
				ammoHolders[o].SetActive(true);
			else 
				ammoHolders[o].SetActive(false);
		}
	}
}