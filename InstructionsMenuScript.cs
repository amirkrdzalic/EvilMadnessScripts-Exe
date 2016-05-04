using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsMenuScript : MonoBehaviour {

	public void Back()
	{
		SceneManager.LoadScene (0);
	}
}
