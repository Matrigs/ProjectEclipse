using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public EnemyController enemyControl;

	public float velocity;
	public GameObject bulletTrail;

	void Start () {
		Effect ();
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

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Wall") {
			//Destroy (bulletTrail);
			Destroy (gameObject);
		}
	}

	void Effect () {
		var obj = Instantiate (bulletTrail, transform.position + new Vector3 (0, 0, 0), Quaternion.identity);
		obj.GetComponent<MoveTrail> ().bulletControl = enemyControl;
	}
}
