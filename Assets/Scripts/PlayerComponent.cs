using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Controller2D))]
public class PlayerComponent : MonoBehaviour
{
	public static PlayerComponent IlioInstance, LunaInstance;

    public enum PlayerCharacter
    {
        Ilio,
        Luna,
        Other
    }

    public delegate void playerAction();
    public event playerAction ActionButton;

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
	[Header("Sounds")]
	public AudioSource walk;
	public AudioSource jump;
	public AudioSource action;
	public AudioSource land;


	[Space]
	[Header("Particle Effects")]
	public GameObject dustParticle;
	private bool spawnDust;


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
    [HideInInspector] public bool IlioActionButtonPressed = false;

	private float lastY;
	public string charname;

	void Awake(){
		switch(character){
			case PlayerCharacter.Ilio:
				if(IlioInstance == null) IlioInstance = this;
				charname = "Ilio";
				break;
			case PlayerCharacter.Luna:
				if(LunaInstance == null) LunaInstance = this;
				charname = "Luna";
				break;
		}
	}

    void Start() {
		controller = GetComponent<Controller2D>();
		pushObjectComponent = GetComponent<PushObject>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		lastY = transform.position.y;
	}
	void OnEnable()
	{		
		//transform.parent.position = spawnPoint.position;
		transform.localPosition = Vector3.zero;
	}

	void Update() {
		RecalculatePhysics(debug);
		CalculateVelocity ();

		controller.Move (velocity * Time.deltaTime, false, animator);
		if (controller.collisionsInf.above || controller.collisionsInf.below) {
			velocity.y = 0;
		}

		var deltaY = transform.position.y - lastY;

		if (deltaY < 0 && animator.GetCurrentAnimatorStateInfo (0).IsName ("JumpUp" + charname)) {
			animator.SetTrigger ("JumpTransition"); 
		}

		if(controller.collisionsInf.below && animator.GetCurrentAnimatorStateInfo(0).IsName("JumpDown" + charname)){ 
			land.Play ();
			animator.SetTrigger ("Jump end"); 
			StartCoroutine(JumpDelay("Jump end"));
			spawnDust = true;
		}

		animator.SetFloat ("DeltaY", deltaY);

		lastY = transform.position.y;

		if (spawnDust == true) {
			Instantiate (dustParticle, transform.position, Quaternion.identity);
			spawnDust = false;
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
			animator.SetBool ("ShieldPlatform", true);
			action.Play ();
        }
		else if (controller.collisionsInf.below) {
			velocity.y = maxJumpVelocity;

			animator.SetTrigger ("Jump");
			jump.Play ();
			StartCoroutine (JumpDelay("JumpTransition"));
		}
	}

	public IEnumerator JumpDelay (string trigger, float duration = 0.4f) {
		yield return new WaitForSeconds (duration);
		animator.SetTrigger(trigger);
	}
	public void OnJumpInputUp()
    {
        if (IlioActionButtonPressed)
        {
			animator.SetBool ("ShieldPlatform", false);
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
		action.Play ();

		if(ActionButton != null) ActionButton();
		StartCoroutine(DoMeleeAttack(activeAttackTime));
		animator.SetBool ("Action", true);
		animator.SetBool ("ShieldPlatform", false);
		if (character == PlayerCharacter.Ilio) {
			IlioActionButtonPressed = true;	
			moveSpeed = moveSpeed - 2;
			ChangePushObjectStatus(true);
		}
    }
	public void OnActionUp(){
		animator.SetBool ("Action", false);
		animator.SetBool ("ShieldPlatform", false);
		if (IlioActionButtonPressed)
		{
			IlioActionButtonPressed = false;
			moveSpeed = moveSpeed + 2;
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
			if (IlioActionButtonPressed) {
				ChangePushObjectStatus (!status);
			} else {
				ChangePushObjectStatus (false);
				animator.SetBool ("ShieldPlatform", false);
			}
		}
    }

	public void ChangePushObjectStatus(bool status){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), 
			LayerMask.NameToLayer("MovableObstacle"), !status);

		if (pushObjectComponent == null)
			return;

		//float correctionAmt = 0.05f;
		//transform.position += (status ? Vector3.right * correctionAmt : Vector3.left  * correctionAmt);
		pushObjectComponent.activated = status;
	}
}