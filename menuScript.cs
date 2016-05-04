using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

	public bool isOffline = false;

	public Canvas quitMenu;
	public Button startText;
	public Button exitText;
	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();


		quitMenu.enabled = false;
	}

	public void ExitPressed()
	{
		quitMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress()
	{
		quitMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
	}

	public void StartLevel()
	{
		isOffline = true;
		if (isOffline == true) {

			Destroy (GameObject.Find ("NetworkManager"));
			Application.LoadLevel (1);
		}
	}

	public void Instructions()
	{
		SceneManager.LoadScene (3);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}
}