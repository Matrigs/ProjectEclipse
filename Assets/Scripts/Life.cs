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
			//Checa se foi parryado
			ParryBox p = info.collider.GetComponent<ParryBox>();
			if(p != null && p.parried) return;

			//Se não, dá dano
			else{
				Debug.Log ("YOU ARE DEAD");
				Destroy (info.gameObject);
				GameMaster.gm.playerPrefabIlio.gameObject.SetActive (false);
				GameMaster.gm.playerPrefabLuna.gameObject.SetActive (false);
				//Destroy (this);
				GameMaster.gm.RespawnPlayer ();
				GameMaster.gm.playerPrefabIlio.gameObject.SetActive (true);
				GameMaster.gm.playerPrefabLuna.gameObject.SetActive (true);
			}
		}

		if (info.collider.tag == "Pit") {
			Debug.Log ("YOU HIT A PIT");
			GameMaster.gm.playerPrefabIlio.gameObject.SetActive (false);
			GameMaster.gm.playerPrefabLuna.gameObject.SetActive (false);
			//Destroy (this);
			GameMaster.gm.RespawnPlayer ();
			GameMaster.gm.playerPrefabIlio.gameObject.SetActive (true);
			GameMaster.gm.playerPrefabLuna.gameObject.SetActive (true);
		}
			
	}

}
