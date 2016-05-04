using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {

	public GameObject PauseMenu;
	bool paused;
	bool muted;
	[SerializeField]
	Text muteText;

	void Start () {
		paused = false;
		muted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.P)) {
			paused = !paused;
		}

		if (paused) {
			PauseMenu.SetActive (true);
			Time.timeScale = 0;
		} else if (!paused) {
			PauseMenu.SetActive(false);
			Time.timeScale = 1;
		}

		if (muted) {
			AudioListener.volume = 0;
			muteText.text = "Unmute";
		} else if (!muted) {
			AudioListener.volume = 1;
			muteText.text = "Mute";
		}
	}


	public void Resume()
	{
		paused = false;
	}

	public void MainMenu()
	{
		Application.LoadLevel (0);
	}

	public void Save()
	{
		PlayerPrefs.SetInt ("currentscenesave", Application.loadedLevel);
	}

	public void Load()
	{
		Application.LoadLevel (PlayerPrefs.GetInt ("currentscenesave"));
	}

	public void Mute()
	{
		muted = !muted;
	}
	public void Quit()
	{
		Application.Quit ();
	}

}