﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour {
	public float pushSpeed = 1f;

	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.layer == LayerMask.NameToLayer ("Hurt")) {
			Vector2 dir = col.GetContact (0).point - (Vector2)transform.position; 
			col.transform.Translate (dir * pushSpeed * Time.deltaTime);
		}
	}
}
