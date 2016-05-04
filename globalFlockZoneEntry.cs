using UnityEngine;
using System.Collections;

public class globalFlockZoneEntry : MonoBehaviour {

	public GameObject flockManagerObj;

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Collider>().tag.Equals ("Player")) {

			flockManagerObj.SetActive (true);
		}
	}
}