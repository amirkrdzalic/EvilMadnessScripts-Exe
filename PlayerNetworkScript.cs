using UnityEngine;
using System.Collections;

public class PlayerNetworkScript : MonoBehaviour 
{
	public GameObject gun;
	public GameObject player;
	public menuScript m_menuScript;

	//public override void OnStartLocalPlayer ()
	//{


		//GameObject camera = GameObject.Find ("Main Camera");
		//camera.GetComponent<CameraController> ().m_MainPlayer = gameObject;
	//}



	// Use this for initialization
	void Start () 
	{


			GameObject env = GameObject.Find ("Environment");
			mainGame game = env.GetComponent<mainGame> (); 
		
				env.GetComponent<mainGame> ().player1 = gameObject;
				env.GetComponent<mainGame> ().player2 = gameObject;
				gun.GetComponent<RayShooting> ().enabled = false;
		 


	}
}
