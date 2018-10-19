using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatablePlatform : ReactiveComponent {

	public PlatformController control;

	void Start () {
		//control = GetComponent<> (PlatformController);
		control.enabled = false;
	}

	public override void Reaction () {
		Debug.Log ("We are now moving. Please board the platform.");
		control.enabled = true;
	}

	
}