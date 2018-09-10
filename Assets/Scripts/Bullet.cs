using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public EnemyController enemyControl;

	public float velocity;

	void Start () {

	}

	void Update () 
	{
		
		if (enemyControl.facingRight == true) {
			transform.Translate(transform.right * velocity * Time.deltaTime);
			Debug.Log ("Enemy Turned Right");
		} 
			
		if (enemyControl.facingRight == false) {
			transform.Translate((transform.right * -1) * velocity * Time.deltaTime);
			Debug.Log ("Enemy Turned Left");
		}

	}
}
