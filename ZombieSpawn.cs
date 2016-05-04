using UnityEngine;
using System.Collections;
//using UnityEngine.Networking; 

public class ZombieSpawn : MonoBehaviour
{
	public GameObject zombiePrefab;

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Player")) 
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Instantiate(zombiePrefab, gameObject.transform.GetChild(i).gameObject.transform.position, Quaternion.identity);
			}
		}
	}
}