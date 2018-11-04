using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour {
	[HideInInspector] public bool activated = false;
	public float pushSpeed = 1f;
	public AudioSource pushing;

	void OnCollisionStay2D(Collision2D col){
		if (activated && (col.gameObject.layer == LayerMask.NameToLayer ("Hurt") || col.gameObject.layer == LayerMask.NameToLayer ("MovableObstacle"))) {
			Vector2 dir = col.GetContact (0).point - (Vector2)transform.position; 
			col.gameObject.GetComponent<Rigidbody2D>().MovePosition((Vector2)col.transform.position + dir * pushSpeed * Time.deltaTime);
		}
	}
}
