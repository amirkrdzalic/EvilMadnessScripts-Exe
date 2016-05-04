using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
    private Vector3 lastplayerposition;
    private float distancetomove;
	public GameObject m_MainPlayer;

	void Start () {
	
	}

	void Update () 
	{
		//old camera - - - - transform.position = new Vector3(m_MainPlayer.transform.position.x +12, transform.position.y, m_MainPlayer.transform.position.z+7);
		transform.position = new Vector3(m_MainPlayer.transform.position.x, transform.position.y, m_MainPlayer.transform.position.z-11);// - lastplayerposition.z;
		//how the fuck do u make the camera not go through the walls
		

	}
	
}