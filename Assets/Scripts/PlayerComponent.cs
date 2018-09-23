using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Controller2D))]
public class PlayerComponent : MonoBehaviour
{
    public enum PlayerCharacter
    {
        Ilio,
        Luna,
        Other
    }

    public delegate void playerAction();
    public static event playerAction ActionButton;

	public Animator animator;

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

	[Header("Character")]
    public PlayerCharacter character;

	[Header("Ilio action")]
	public GameObject shieldPlatform;
	private PushObject pushObjectComponent;
    private bool IlioActionButtonPressed = false;

    void Start() {
		controller = GetComponent<Controller2D>();
		pushObjectComponent = GetComponent<PushObject>();

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
		//animator.SetFloat ("Speed", Mathf.Abs(velocityXSmoothing));

		controller.Move (velocity * Time.deltaTime, false, animator);
		if (controller.collisionsInf.above || controller.collisionsInf.below) {
			velocity.y = 0;
		}
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;

	}

	public void OnJumpInputDown() 
	{
        if (IlioActionButtonPressed)
        {
            IlioAction(true);
        }
		else if (controller.collisionsInf.below) {
			velocity.y = maxJumpVelocity;
		}
	}

	public void OnJumpInputUp()
    {
        if (IlioActionButtonPressed)
        {
        }
        else if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
		IlioAction(false);
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
		animator.SetBool ("Action", true);

		if (character == PlayerCharacter.Ilio) {
			IlioActionButtonPressed = true;	
			ChangePushObjectStatus(true);
		}
    }
	public void OnActionUp(){
		animator.SetBool ("Action", false);
		if (IlioActionButtonPressed)
		{
			IlioActionButtonPressed = false;
			IlioAction(false);
			ChangePushObjectStatus(false);
		}		
	}
	IEnumerator DoMeleeAttack(float activeTime)
	{
		meleeAtt.startCheckingCollision();
		Debug.Log("ATTACK");
		yield return new WaitForSeconds (activeTime);
		meleeAtt.stopCheckingCollision();
		Debug.Log("STOP ATTACK");
		if(character != PlayerCharacter.Ilio) animator.SetBool ("Action", false);
	}

    public void IlioAction(bool status)
    {
		if (character == PlayerCharacter.Ilio) {
			shieldPlatform.SetActive (status);
			animator.SetBool ("ShieldPlatform", status);

			if (IlioActionButtonPressed)
				ChangePushObjectStatus (!status);
			else
				ChangePushObjectStatus (false);
		}
    }

	public void ChangePushObjectStatus(bool status){
		if (pushObjectComponent == null)
			return;

		pushObjectComponent.activated = status;
	}
}