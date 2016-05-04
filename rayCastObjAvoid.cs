using UnityEngine;
using System.Collections;

public class rayCastObjAvoid : MonoBehaviour {

	public float speed;
	public GameObject player;
	private Vector3 dir;
	private Vector3 dirFull;
	public float rayDistance;
	//public float const steep = 0.1f;

	// Use this for initialization
	void Start () {
		rayDistance = 3.0f;
		speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

		//directional vector to target
		dir = (player.transform.position - transform.position).normalized;
		RaycastHit hit;

		//check forward rayCast
		if (Physics.Raycast (transform.position, transform.forward, out hit, rayDistance)) {
			Debug.DrawLine (transform.position, hit.point, Color.green);
			dir += hit.normal * 20.0f; //20 if force to repel by
		} else {
			Debug.DrawLine (transform.position, transform.forward * rayDistance, Color.yellow);
		}

		float z = transform.rotation.eulerAngles.z;

		Vector3 leftRayCast = transform.position + new Vector3 (Mathf.Cos(-Mathf.PI/4f+z), 0, Mathf.Sin(-Mathf.PI/4f+z));
		Vector3 rightRayCast = transform.position + new Vector3 (Mathf.Cos(Mathf.PI/4f+z), 0, Mathf.Sin(Mathf.PI/4f+z));

		//check left ray
		if (Physics.Raycast (leftRayCast, transform.forward, out hit, 2)) {
			if (hit.collider.gameObject != player) {
				if (hit.collider != null) {
					Debug.DrawLine (rightRayCast, hit.point, Color.red, 5.0f);
					dir += hit.normal * 20;
				}
			}
		}
		//check right ray
		if (Physics.Raycast (rightRayCast, transform.forward, out hit, 2)) {
			if (hit.collider.gameObject != player) {
				if (hit.collider != null) {
					Debug.DrawLine (rightRayCast, hit.point, Color.white, 5.0f);
					dir += hit.normal * 10;
				}
			}
		}
		//movement

		//rotation
		Quaternion rot = Quaternion.LookRotation (dir);
		transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime);
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
