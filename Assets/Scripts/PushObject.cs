using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour {
	public float pushSpeed = 1f;

	void OnCollisionStay2D(Collision2D col){
		Debug.Log ("bep (" + LayerMask.NameToLayer ("Hurt") + "," + col.gameObject.layer + ")");
		if (col.gameObject.layer == LayerMask.NameToLayer ("Hurt")) {
			Debug.Log ("bip");
			Vector2 dir = col.GetContact (0).point - (Vector2)transform.position; 
			col.transform.Translate (dir * pushSpeed * Time.deltaTime);
		}
	}
}
