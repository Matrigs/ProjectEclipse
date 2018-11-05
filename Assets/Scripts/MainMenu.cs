using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Animator fadeAnimator;
	public Animator buttonAnimator;
	public GameObject logo;
	public GameObject mainMenuUI;
	public GameObject creditsScreenUI;
	//public GameObject lights;
	public int fadeDelay = 1;
	public bool activateCredits = false;
	public bool activateBackToMain = false;

	public void Start () {
		Time.timeScale = 1;
		//buttonAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	public void PlayGame () {
		StartCoroutine (PlayGameCoroutine ());
	}

	public IEnumerator PlayGameCoroutine(){
		yield return new WaitForSeconds (fadeDelay);
		fadeAnimator.SetTrigger ("FadeOut");
		yield return new WaitForSeconds (fadeDelay);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	public void Credits () {
		Time.timeScale = 0f;
		//StartCoroutine (WaitForRealSeconds(fadeDelay));
		fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		fadeAnimator.SetTrigger ("FadeOut");

		activateCredits = true;

		StartCoroutine (fadeInDelayer ());
	}

	public void QuitGame () {
		Debug.Log ("Quit!");
		Application.Quit ();
	}

	public void Back () {
		Time.timeScale = 0f;
		fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		fadeAnimator.SetTrigger ("FadeOut");

		activateBackToMain = true;

		StartCoroutine (fadeInDelayer ());
	}

	IEnumerator fadeInDelayer () {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + fadeDelay) {
			yield return null;
		}
		Time.timeScale = 1f;

		if (activateCredits == true) {
			logo.SetActive (false);
			mainMenuUI.SetActive (false);
			creditsScreenUI.SetActive (true);
			activateCredits = false;
		}
			
		if (activateBackToMain == true) {
			creditsScreenUI.SetActive (false);
			logo.SetActive (true);
			mainMenuUI.SetActive (true);
			activateBackToMain = false;
		}
			
		fadeAnimator.SetTrigger ("FadeIn");
		fadeAnimator.updateMode = AnimatorUpdateMode.Normal;
	}
}
