using UnityEngine;
using System.Collections;

public class globalFlock : MonoBehaviour {
	
	//variables for FLOCKING....
	public GameObject zombieFlockerPrefab;
	public static int size = 10; //tank size for fish bascially
	static int numOfZombies = 10;
	public static GameObject[] allZombies = new GameObject[numOfZombies];
	public static Vector3 goalpos = Vector3.zero;

	void Start () {
		for (int i = 0; i < numOfZombies; i++) 
		{
			//add this.transform.position to start at empty object spawn area
			Vector3 pos = this.transform.position + new Vector3 (Random.Range(0, 5),
										0,
										Random.Range(0, 5)); //x,y,z randomness
			allZombies[i] = (GameObject) Instantiate(zombieFlockerPrefab, pos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10000) < 50) {
			goalpos = new Vector3 (Random.Range(-size, size),
									0,
									Random.Range(-size, size));
		}
	}
}
