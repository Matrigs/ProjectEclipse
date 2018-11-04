using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAhead : MonoBehaviour {
	public float distance = 10f;
	[Range(0,1)]public float speed = 0.1f;
	public Transform[] Targets;
	private Vector3[] _targetsCurPos;
	private bool lookAheadCoroutineActive = false;
	private int leftMostIdx = 0, rightMostIdx = 0;

	// Use this for initialization
	void Start () {
		_targetsCurPos = new Vector3[Targets.Length];

		UpdatePos();
	}

	void CheckPos(){
		bool moveLeft = false, moveRight = false;

		//checando os elementos de cada ponta
		for(int i = 0; i < Targets.Length; i++){
			if(Targets[i].position.x < Targets[leftMostIdx].position.x) leftMostIdx = i;
			if(Targets[i].position.x > Targets[rightMostIdx].position.x) rightMostIdx = i;
		}

		//checando se algum dos dois se moveu
		if(Targets[leftMostIdx].position.x < _targetsCurPos[leftMostIdx].x) moveLeft = true;
		if(Targets[rightMostIdx].position.x > _targetsCurPos[rightMostIdx].x) moveRight = true;

		Debug.Log(moveLeft + "," + moveRight);

		//move o look ahead apenas se só uma das variáveis é verdadeira
		if((!moveLeft && !moveRight) || (moveLeft && moveRight)) return;

		transform.position = distance*(moveLeft ? Vector3.left : Vector3.right) 
			+ Targets[(moveLeft ? leftMostIdx : rightMostIdx)].position;

		if(!lookAheadCoroutineActive) StartCoroutine(lookAheadCoroutine());
	}

	public IEnumerator lookAheadCoroutine(){
		lookAheadCoroutineActive = true;

		while(transform.position.x > Targets[rightMostIdx].position.x){
				transform.position = Vector3.Lerp(transform.position, Targets[rightMostIdx].position, speed);
				yield return new WaitForEndOfFrame();
			}

		while(transform.position.x < Targets[leftMostIdx].position.x){
				transform.position = Vector3.Lerp(transform.position, Targets[leftMostIdx].position, speed);
				yield return new WaitForEndOfFrame();
		}

		lookAheadCoroutineActive = false;
	}

	void UpdatePos(){
		for(int i = 0; i < Targets.Length; i++){
			_targetsCurPos[i] = Targets[i].position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckPos();

		UpdatePos();
	}
}
