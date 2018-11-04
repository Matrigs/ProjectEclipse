using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHitBox : MonoBehaviour {
	public Transform HitBox;

	public Controller2D playerController2D;

	private Color _defaultColor;
	private SpriteRenderer _spriteRend;

	void Start(){
		_spriteRend = gameObject.GetComponent<SpriteRenderer>();
		if(_spriteRend != null) _defaultColor = _spriteRend.color;
	}

	void Update () 
	{
		GetComponent<SpriteRenderer>().flipX = !playerController2D.facingRight;

		transform.position = HitBox.position;
		Vector3 newRotation = Vector3.zero;
		newRotation.z = playerController2D.collisionsInf.slopeAngle;

		transform.rotation = Quaternion.Euler(newRotation);
	}

	public void Reset(){
		if(_spriteRend != null) _spriteRend.color = _defaultColor;
	}
}
