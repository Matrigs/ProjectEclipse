using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	//static Vector3 ReachedPoint;
	//public PlayerComponent playerComponent;

	//public GameMaster gamespawn;
	List <Collider2D> players = new List<Collider2D>();
	public GameObject emblem;
	public AudioSource check1;
	public AudioSource check2;
	public AudioSource check3;


	// Use this for initialization
	void Start () {
		//spawnPoint = GameObject.FindGameObjectWithTag ("Spawnpoint");
		emblem.SetActive (false);
		Debug.Log ("Checkpoints Reset");
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (!players.Contains(other.GetComponent<Collider2D>()) && other.GetComponent<Collider2D>().tag == "Player") {
			//old conditions for the if statement
			//other.CompareTag("Player")
			//PlayerComponent.PlayerCharacter == PlayerComponent.PlayerCharacter.Ilio && PlayerComponent.PlayerCharacter == PlayerComponent.PlayerCharacter.Luna
			players.Add(other.GetComponent<Collider2D>());
		}
	}

	void OnTriggerExit2D() {
		players.Clear ();
	}

	void Update () { 
		if (players.Count == 2 && !IsThisTheLastCheckpoint()) {
			Debug.Log ("Checkpoint Reached!");
			emblem.SetActive (true);
			check1.Play ();
			check2.Play ();
			check3.Play ();
			GameMaster.gm.checkpoints.Add(transform);
			GameMaster.gm.state = new GameState();
		}
	}

    bool IsThisTheLastCheckpoint(){
		return GameMaster.gm.checkpoints[GameMaster.gm.checkpoints.Count - 1] == transform;
	}
}
