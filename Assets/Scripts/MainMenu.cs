using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Animator fadeAnimator;
	public GameObject logo;
	public GameObject mainMenuUI;
	public GameObject creditsScreenUI;
	public int fadeDelay = 2;

	public void PlayGame () {
		fadeAnimator.SetTrigger ("FadeOut");

		StartCoroutine (WaitForRealSeconds(fadeDelay));

		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	public void Credits () {
		fadeAnimator.SetTrigger ("FadeOut");

		StartCoroutine (WaitForRealSeconds(fadeDelay));
		logo.SetActive (false);
		mainMenuUI.SetActive (false);
		creditsScreenUI.SetActive (true);

		fadeAnimator.SetTrigger ("FadeIn");
	}

	public void QuitGame () {
		Debug.Log ("Quit!");
		Application.Quit ();
	}

	public void Back () {
		fadeAnimator.SetTrigger ("FadeOut");

		StartCoroutine (WaitForRealSeconds(fadeDelay));
		creditsScreenUI.SetActive (false);
		logo.SetActive (true);
		mainMenuUI.SetActive (true);

		fadeAnimator.SetTrigger ("FadeIn");
	}
		
	public static IEnumerator WaitForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}
}
