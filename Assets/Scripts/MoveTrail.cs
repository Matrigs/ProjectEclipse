using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

	public int moveSpeed = 230;
	public EnemyController bulletControl;

	// Update is called once per frame
	void Update () {
		//transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);

		if (bulletControl.facingRight == true) {
			transform.Translate(transform.right * moveSpeed * Time.deltaTime);
			Debug.Log ("Particle Right");
		} 

		if (bulletControl.facingRight == false) {
			transform.Translate((transform.right * -1) * moveSpeed * Time.deltaTime);
			Debug.Log ("Particle Left");
		}
	}
}
