using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameMaster.gm.blocks.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
