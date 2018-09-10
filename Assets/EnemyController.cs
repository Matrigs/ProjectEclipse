using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public Transform HitBox;
	public bool facingRight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().flipX = !facingRight;

		transform.position = HitBox.position;
		Vector3 newRotation = Vector3.zero;

		transform.rotation = Quaternion.Euler(newRotation);
	}


	public void ChangeEnemyDir()
	{
		facingRight = !facingRight;

	}

}
