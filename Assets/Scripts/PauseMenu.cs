using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using InControl;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;
	public GameObject FirstOptionToSelect;

	private InputDevice[] devices;

	// Use this for initialization
	void Start () {
		//checar se o incontroller está ativado (se sim, pegar a referência do controle)
		if(InputManager.Devices.Count > 0){
			devices = new InputDevice[InputManager.Devices.Count];
			int i = 0;

			foreach (InputDevice d in InputManager.Devices){
				devices[i] = d;
				i++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//se o incontroller estiver ativado, permitir sair e navegar pelo menu com o controle

		foreach(InputDevice device in devices){
			if(device.CommandIsPressed){
				if (GameIsPaused) {
					Resume ();
				} else {
					Pause ();
				}	
			}
		}


		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.P)) {
			if (GameIsPaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	public void Resume () {
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void Pause () {
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
		EventSystem.current.SetSelectedGameObject(FirstOptionToSelect);
	}

	public void LoadMenu () {
		Debug.Log ("Loading Menu...");
		SceneManager.LoadScene ("Main Menu");
	}

	public void QuitGame () {
		Debug.Log ("Quitting game...");
		Application.Quit ();
	}

}
