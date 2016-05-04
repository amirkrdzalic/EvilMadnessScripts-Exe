using UnityEngine;
using System.Collections;

public class SprintScript : MonoBehaviour {

	float timer = 150;

	PlayerMovement playerMTry_script;
	// Use this for initialization
	void Start () {
	
		playerMTry_script = gameObject.GetComponent<PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

		while (Input.GetKey(KeyCode.LeftShift)) {
			timer--;
			if (timer <=0)
			{
				break;
			}
			playerMTry_script.speed = 0.75f;
		}
		playerMTry_script.speed = 0.35f;
	
	}
}
