using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
	public Transform playerPrefabIlio;
	public Transform playerPrefabLuna;
	public Transform spawnPoint;

	public List <Transform> checkpoints = new List<Transform> ();

	public Vector3 spawnPosition {get {return checkpoints[checkpoints.Count - 1].position;}}

	void Start () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			checkpoints.Add(spawnPoint);
		}
	}

	//public int spawnDelay;

	public void RespawnPlayer () {
		playerPrefabIlio.transform.position = spawnPosition + Vector3.left;
		playerPrefabLuna.transform.position = spawnPosition + Vector3.right;

		/*
		for (int i = 0; i < playerPrefabIlio.transform.childCount; i++) {
			playerPrefabIlio.transform.GetChild (i).transform.position = spawnPosition;
		}

		for (int i = 0; i < playerPrefabLuna.transform.childCount; i++) {
			playerPrefabLuna.transform.GetChild (i).transform.position = spawnPosition;
		}
		*/

		Debug.Log (playerPrefabIlio.transform.position + " PlayerPrefabIlio");
		Debug.Log (playerPrefabLuna.transform.position + " PlayerPrefabLuna");
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
