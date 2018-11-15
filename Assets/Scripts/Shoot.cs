using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class Shoot : MonoBehaviour {

	public GameObject Bullet;
	public Vector3 BulletSpawnPoint = Vector3.zero;
	//public GameObject MoveTrail;

	public float timeOfShots;
	public AudioSource shotNoise;
	public Animator enemyAnimator;
	public EnemyController dirControl;

	void Start () 
	{
		StartCoroutine(ShootRoutine());
	}
	private IEnumerator ShootRoutine()
	{
		FieldOfView fov = GetComponent<FieldOfView>();

		for(;;)
		{
			//enemyAnimator.SetBool ("Shot", false);
			yield return new WaitForSeconds(timeOfShots);
			if (fov.visibleTargets.Count > 0) {
				enemyAnimator.SetTrigger ("Shot 0");

			}
		}

	}

	public void Fire(){

		var obj = Instantiate (Bullet, transform.position + BulletSpawnPoint, Quaternion.identity);
		//obj.GetComponent<MoveTrail> ().bulletControl = dirControl;
		obj.GetComponent<Bullet> ().enemyControl = dirControl;
		shotNoise.Play ();
	}

	public void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position + BulletSpawnPoint, 0.25f);
	}

}
