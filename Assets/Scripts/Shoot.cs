using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject Bullet;
	public float timeOfShots;
	void Start () 
	{
		StartCoroutine(ShootRoutine());
	}
	private IEnumerator ShootRoutine()
	{
		for(;;)
		{
			yield return new WaitForSeconds(timeOfShots);
			Instantiate(Bullet,transform.position + new Vector3(0,0.2f,0),Quaternion.identity);
		}
	}
}
