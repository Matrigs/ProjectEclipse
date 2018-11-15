using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

	public Animator animator;
	public int spawnDelay = 2;

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
				FindObjectOfType<AudioManager> ().Play ("Death");

				Time.timeScale = 0f;
				animator.updateMode = AnimatorUpdateMode.UnscaledTime;
				animator.SetBool ("Death", true);

				Debug.Log("Spawn delayer");
				StartCoroutine (SpawnDelayer());
			}
		}

		if (info.collider.tag == "Pit") {
			Debug.Log ("YOU HIT A PIT");
			FindObjectOfType<AudioManager> ().Play ("Death");

			Time.timeScale = 0f;
			animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			animator.SetBool ("Death", true);

			StartCoroutine (SpawnDelayer());
		}
	}

	IEnumerator SpawnDelayer () {
		yield return StartCoroutine (WaitForRealSeconds(spawnDelay));
		Time.timeScale = 1f;

		GameMaster.gm.playerPrefabIlio.gameObject.SetActive (false);
		GameMaster.gm.playerPrefabLuna.gameObject.SetActive (false);

		GameMaster.gm.RespawnPlayer ();
		GameMaster.gm.playerPrefabIlio.gameObject.SetActive (true);
		GameMaster.gm.playerPrefabLuna.gameObject.SetActive (true);


		animator.SetBool ("Death", false);
		animator.updateMode = AnimatorUpdateMode.Normal;

		//reset color
		//GameMaster.gm.playerPrefabIlio.transform.Find("PlayerHitBox").GetComponent<FollowHitBox>().Reset();
		//GameMaster.gm.playerPrefabLuna.transform.Find("PlayerHitBox").GetComponent<FollowHitBox>().Reset();
	}

	public static IEnumerator WaitForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}

}
