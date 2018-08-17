using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour 
{
	public static GameMaster gm;
	public GameObject Player;
	
	[Header("REVIEW THIS")]
	[Space]
	Vector3 spawnPosition;
	public Transform playerPrefabIlio;
	public Transform playerPrefabLuna;
	public Transform spawnPoint;
	private bool _respawning;

	void Start () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			//spawnPosition.Set (spawnPoint.position.x, spawnPoint.position.y, 0);
		}
	}
	void Update()
	{
		if(!Player.activeInHierarchy)
		{
			if(!_respawning) 
			{
				Debug.Log("Respawning");
				StartCoroutine(RespawnPlayerWithDelay(0.5f));
			}
		}
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
	public IEnumerator RespawnPlayerWithDelay(float delay)
	{
		_respawning = true;
		yield return new WaitForSeconds(delay);
		Player.SetActive(true);
		_respawning = false;
	}
	/*
	public static void KillPlayer (PlayerComponent player) {
		Destroy (player.gameObject);
		gm.RespawnPlayer ();
	}
	*/

}
