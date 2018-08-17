using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float velocity;
	void Update () 
	{
		transform.Translate(transform.right * velocity * Time.deltaTime);
	}
}
