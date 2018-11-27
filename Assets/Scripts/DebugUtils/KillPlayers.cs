using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayers : MonoBehaviour {
	public KeyCode killKey;
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (killKey)) {
			Life l = PlayerComponent.IlioInstance.gameObject.GetComponent<Life> ();
			l.animator.SetBool ("Death", true);
			StartCoroutine (l.SpawnDelayer ());
		}
	}
}
