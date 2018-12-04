using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEnd : MonoBehaviour {

	public Animator fadeAnimator;
	public int fadeDelay = 2;
	public Camera main;

	private List<GameObject> _playersOnTrigger =  new List<GameObject>();

	private void OnEnable()
	{
		//_levelEndTrigger = GetComponent<Collider2D>();
	}

	private void OnPlayersLevelEnd(List<GameObject> _playersOnTrigger)
	{
		if(_playersOnTrigger.Count == 2) // If the number of player in the Level End trigger is Equal to 2
		{
			Time.timeScale = 0f;
			fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
			fadeAnimator.SetTrigger ("FadeOut");

			StartCoroutine (fadeInDelayer ());
		}
	}

	void OnTriggerExit2D()
	{
		_playersOnTrigger.Clear();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Other" + other.name + "Tag" + other.tag);
		if (!_playersOnTrigger.Contains(other.gameObject) && other.tag == "Player")
		{
			_playersOnTrigger.Add(other.gameObject);
			OnPlayersLevelEnd(_playersOnTrigger);
		}
	}

	IEnumerator fadeInDelayer () {
		yield return StartCoroutine (WaitForRealSeconds(fadeDelay/2));
		Time.timeScale = 1f;

		SceneManager.LoadScene ("Credits");

		yield return StartCoroutine (WaitForRealSeconds(fadeDelay/2));

		fadeAnimator.SetTrigger ("FadeIn");
		fadeAnimator.updateMode = AnimatorUpdateMode.Normal;
	}

	public static IEnumerator WaitForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}
}
