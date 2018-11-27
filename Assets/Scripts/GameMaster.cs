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
	public Checkpoint curCheckpoint {get {return checkpoints[checkpoints.Count - 1].GetComponent<Checkpoint>();}}

	public float maxDistanceBetweenPlayers = 10f;
	private float curDistanceBetweenPlayers;

	//Objetos que os checkpoints guardarão informações sobre
	[HideInInspector] public List<DoorComponent> doors;
	[HideInInspector] public List<ButtonComponent> buttons;
	[HideInInspector] public List<EnemyController> enemies;
	[HideInInspector] public List<Block> blocks;

	[HideInInspector] public GameState state;

	void Awake () {
		if (gm == null) {
			gm = this; //GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			checkpoints.Add(spawnPoint);
		}

		doors = new List<DoorComponent>();
		buttons = new List<ButtonComponent>();
		enemies = new List<EnemyController>();
		blocks = new List<Block>();
	}

	void Start(){
		state = new GameState();
	}

	//public int spawnDelay = 2;

	public void RespawnPlayer () {
		//yield return new WaitForSeconds (spawnDelay);

		state.RestoreState();

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
