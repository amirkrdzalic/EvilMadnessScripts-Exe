using UnityEngine;
using System.Collections;

public class LightFlickering : MonoBehaviour {

	public Light flashinglight;

	void FixedUpdate()
	{
		float RandomNumber = Random.value;
		if (RandomNumber <= .7) {
			flashinglight.enabled = true;
		} else {
			flashinglight.enabled = false;
		}
	}
}