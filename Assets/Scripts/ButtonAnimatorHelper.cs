using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimatorHelper : MonoBehaviour {

	public void PlayHighlighted () {

		AudioManager.instance.Play ("Select");
	}
}
