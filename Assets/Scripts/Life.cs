using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

	/*
	[System.Serializable]
	public class PlayerStats {
		public int Health = 100;
	}

	public PlayerStats playerStats = new PlayerStats(); 


	public int fallBoundary = -20;

	void Update () {
		if (transform.position.y <= fallBoundary) {
			DamagePlayer (9999999);
		}
	}

	public void DamagePlayer (int damage) {
		playerStats.Health -= damage;
		if (playerStats.Health <= 0) {
			Debug.Log ("KILL PLAYER");
			GameMaster.KillPlayer (this);
		}
	}
	*/


	void OnCollisionEnter2D (Collision2D info) {
		if (info.collider.tag == "Danger") {
			Debug.Log ("YOU ARE DEAD");
			gameObject.SetActive (false);
			//Destroy (this);
			GameMaster.gm.RespawnPlayer ();
			gameObject.SetActive (true);
		}
			
	}

}
