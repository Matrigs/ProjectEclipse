using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject Bullet;
	public float timeOfShots;

	public EnemyController dirControl;

	void Start () 
	{
		StartCoroutine(ShootRoutine());
	}
	private IEnumerator ShootRoutine()
	{
		for(;;)
		{
			yield return new WaitForSeconds(timeOfShots);
			var obj = Instantiate (Bullet, transform.position + new Vector3 (0, 0.2f, 0), Quaternion.identity);

			obj.GetComponent<Bullet> ().enemyControl = dirControl;
		}

	}
}
