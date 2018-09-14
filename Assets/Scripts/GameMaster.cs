using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
	Vector3 spawnPosition;
	public Transform playerPrefabIlio;
	public Transform playerPrefabLuna;
	public Transform spawnPoint;

	//public GameObject checkPoint;

	void Start () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			spawnPosition.Set (spawnPoint.position.x, spawnPoint.position.y, 0);
		}

		//checkPoint.SetActive (false);
	}

	//public int spawnDelay;

	public void RespawnPlayer () {
		playerPrefabIlio.transform.position = spawnPosition;
		playerPrefabLuna.transform.position = spawnPosition;

		for (int i = 0; i < playerPrefabIlio.transform.childCount; i++) {
			playerPrefabIlio.transform.GetChild (i).transform.position = spawnPosition;
		}

		for (int i = 0; i < playerPrefabLuna.transform.childCount; i++) {
			playerPrefabLuna.transform.GetChild (i).transform.position = spawnPosition;
		}

		Debug.Log (playerPrefabIlio.transform.position + " PlayerPrefabIio");
		Debug.Log (playerPrefabLuna.transform.position + " PlayerPrefabIio");
		Debug.Log (spawnPosition + " SpawnPosition");
		Debug.Log ("TO DO: Add Spawn Particles");
	}
	/*
	public static void KillPlayer (PlayerComponent player) {
		
			Destroy (player.gameObject);
		
			gm.RespawnPlayer ();
	
		}
	
	*/
}
