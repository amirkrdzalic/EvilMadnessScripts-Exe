using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class finishedGame : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Collider>().gameObject.tag.Equals("Player"))
		{
			SceneManager.LoadScene (2);
		}
	}
}
