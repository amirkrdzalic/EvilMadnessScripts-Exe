using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverSceneScript : MonoBehaviour {

	public GameObject GameOverMenu;

	public void Retry()
	{
		Application.LoadLevel (1);
	}

	public void Load()
	{
		Application.LoadLevel (PlayerPrefs.GetInt ("currentscenesave"));
	}

	public void MainMenu()
	{
		Application.LoadLevel (0);
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
