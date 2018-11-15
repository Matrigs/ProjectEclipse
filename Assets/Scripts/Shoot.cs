using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class Shoot : MonoBehaviour {

	public GameObject Bullet;
	//public GameObject MoveTrail;

	public float timeOfShots;
	public AudioSource shotNoise;

	public EnemyController dirControl;

	void OnEnable () 
	{
		StartCoroutine(ShootRoutine());
		Debug.Log("A");
	}
	private IEnumerator ShootRoutine()
	{
		FieldOfView fov = GetComponent<FieldOfView>();

		for(;;)
		{
			yield return new WaitForSeconds(timeOfShots);
			if (fov.visibleTargets.Count > 0) {
				var obj = Instantiate (Bullet, transform.position + new Vector3 (0, 0, 0), Quaternion.identity);
				//obj.GetComponent<MoveTrail> ().bulletControl = dirControl;
				obj.GetComponent<Bullet> ().enemyControl = dirControl;
				shotNoise.Play ();

			}
		}

	}

}
