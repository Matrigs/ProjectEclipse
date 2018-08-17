using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHitBox : MonoBehaviour {
	public Transform HitBox;

	public Controller2D playerController2D;

	void Update () 
	{
		GetComponent<SpriteRenderer>().flipX = !playerController2D.facingRight;

		transform.position = HitBox.position;
		Vector3 newRotation = Vector3.zero;
		newRotation.z = playerController2D.collisionsInf.slopeAngle;

		transform.rotation = Quaternion.Euler(newRotation);
	}
}
