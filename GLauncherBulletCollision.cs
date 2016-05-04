using UnityEngine;
using System.Collections;

public class GLauncherBulletCollision : MonoBehaviour {

	public Rigidbody GLbullet_rigidbody;
	public Transform Explosion;
	public float speed = 50.0f;
	public float power = 5.0f;
	public float radius = 1.0f;
	public bool hasCollided = false; //used for the explosion soundEFX....
	public AudioSource explono;

	void Start () {
		//shoots bullet with force after spawned, in direction your looking.
		GLbullet_rigidbody.velocity = transform.TransformDirection (Vector3.forward * speed);
	}

	void Update () {
		//if bullet didnt hit anything then destroy it at this time, minimizing the distance travelled
		Destroy (gameObject, 0.5f);
	}

	void OnCollisionEnter(Collision otherCollider)
	{
		GLbullet_rigidbody.velocity = Vector3.zero; //when collision occurs, shut off its velocity so you can know the exact position of the collision point.
		Vector3 explosionPos = transform.position; //explosion position at that point ^^^
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); //everything that got hit, put in an array

		Destroy (gameObject); //destroy it cuz it doesnt need to exist anymore
		//audio
		//explono.Play();

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();
			//explosion effect
			var clone = Instantiate (Explosion, explosionPos, Quaternion.identity);
			//destroy explosion effect after 0.3f seconds
			Destroy (clone, 0.3f);
			if (rb != null)
			{
				if (hit.gameObject.tag.Equals ("Zombie")) {
					//deal DMG
					GameObject Zombie = hit.GetComponent<Collider> ().gameObject;
					Zombie.GetComponent<ZombieHealth> ().z_gotHit = true;
					Zombie.GetComponent<ZombieHealth> ().ApplyDamageOnZombie ();
					//addforce to any R.B. hit
					rb.AddExplosionForce (power, explosionPos, radius, 0.0f, ForceMode.Impulse);
				}
			}
		}
	}
}