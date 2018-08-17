using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Controller2D))]
public class PlayerComponent : MonoBehaviour {

    public delegate void playerAction();
    public static event playerAction ActionButton;

    [Header("Debug?")]
	public bool debug;

	[Header("Movement Variables")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float fallMultipler = 2.5f;
	[Space]
	public float accelerationTimeAirborne = .2f;	
	public float accelerationTimeGrounded = .1f;
	[Range(0.1f,50f)]
	public float moveSpeed = 6;
	[Header("MELEE ATTACK")]
	public MeleeAttack meleeAtt;
	public float activeAttackTime = 0.5f;


    [Space]
    [Header("PRIVATE DEBUG SHIT")]
	[SerializeField]
	float gravity;
	[SerializeField]
	float maxJumpVelocity;
	[SerializeField]
	float minJumpVelocity;
	[SerializeField]
	[Space]
	public float velocityXSmoothing;

    [SerializeField]
	Vector3 velocity;

	Controller2D controller;

	Vector2 directionalInput;
	[SerializeField]
	[Space]
	private Transform spawnPoint;

	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}
	void OnEnable()
	{		
		transform.parent.position = spawnPoint.position;
		transform.localPosition = Vector3.zero;
	}

	void Update() {
		RecalculatePhysics(debug);
		CalculateVelocity ();

		controller.Move (velocity * Time.deltaTime);
		if (controller.collisionsInf.above || controller.collisionsInf.below) {
			velocity.y = 0;
		}
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() 
	{		
		if (controller.collisionsInf.below) {
			velocity.y = maxJumpVelocity;
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisionsInf.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		if(velocity.y < 0f)
		{
			velocity.y += gravity * fallMultipler * Time.deltaTime;
		}
		else
		{			
			velocity.y += gravity * Time.deltaTime;
		}
	}
    void RecalculatePhysics(bool debug)
    {
        if (debug)
        {
            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        }
    }
    public void OnActionDown()
    {
        ActionButton();
		StartCoroutine(DoMeleeAttack(activeAttackTime));
		
    }
	IEnumerator DoMeleeAttack(float activeTime)
	{
		meleeAtt.startCheckingCollision();
		Debug.Log("ATTACK");
		yield return new WaitForSeconds(activeTime);
		meleeAtt.stopCheckingCollision();
		Debug.Log("STOP ATTACK");
	}
}