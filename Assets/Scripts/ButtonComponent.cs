using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ButtonComponent : InteractiveComponent
{
    public ReactiveComponent attachedReactionObject;
	public ReactiveComponent attachedReactionObject2;
    public buttonState currentState = buttonState.active;
	public GameObject diamond;
	public GameObject buttonParts;
	public AudioSource crystal;

    [SerializeField]
    private bool ilioPresence, lunaPresence;
    [SerializeField]
    private List<ButtonMaterial> ButtonMaterials = new List<ButtonMaterial>(2);

    public ParticleSystem ThisObjectParticle
    {
        get
        {
            return this.GetComponent<ParticleSystem>();
        }
    }

    [System.Serializable]
    public struct ButtonMaterial
    {
        public buttonState state;
        public Material material;
    }
    public enum buttonState {active, inactive}

    private void OnEnable(){
		if(PlayerComponent.IlioInstance != null) StartCoroutine(DelayStart());
    }

	public IEnumerator DelayStart(){
		while (GameMaster.gm == null || GameMaster.gm.buttons == null) {
			Debug.Log (GameMaster.gm);
			yield return new WaitForEndOfFrame ();
		}

		Start ();
	}
    
    private void Start()
    {
		if(!GameMaster.gm.buttons.Contains(this)) GameMaster.gm.buttons.Add(this);

        PlayerComponent.LunaInstance.ActionButton += LunaAction;
    }
    private void OnDisable()
    {
        PlayerComponent.LunaInstance.ActionButton -= LunaAction;
    }

    public void LunaAction(){
        if(lunaPresence) Action();
    }

    public override void Action()
    {
        //base.Action();
        if (currentState == buttonState.active)
        {
            Debug.Log("DoorOpen");
            ChangeState();
			diamond.SetActive (false);
			crystal.Play ();

            //ActivateParticle(ThisObjectParticle);
			Instantiate (buttonParts, diamond.transform.position, transform.rotation * Quaternion.Euler(-180,0,0));
			ParticleSystem parts = buttonParts.GetComponent<ParticleSystem> ();

			//Rever depois pra não ficar tão cagado
			//float totalDuration = parts.main.duration * 10;
			//Destroy (buttonParts, totalDuration);

            attachedReactionObject.Reaction();
			if(attachedReactionObject2 != null) attachedReactionObject2.Reaction();
        }
    }
    public void ChangeState()
    {
        if(currentState == buttonState.active)
        {
            currentState = buttonState.inactive;
        }
        else
        {
            currentState = buttonState.active;
        }
        foreach(ButtonMaterial bmat in ButtonMaterials)
        {
            if(bmat.state == currentState)
            {
                if(GetComponent<MeshRenderer>()) GetComponent<MeshRenderer>().material = bmat.material;
            }
		}
		transform.Find ("Cristal").gameObject.SetActive (currentState == buttonState.active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hero");
        if (collision.CompareTag("Player"))
        {
            PlayerComponent.PlayerCharacter character = collision.gameObject.GetComponent<PlayerComponent>().character;
            if(character == PlayerComponent.PlayerCharacter.Ilio) ilioPresence = true;
            if(character == PlayerComponent.PlayerCharacter.Luna) lunaPresence = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerComponent.PlayerCharacter character = collision.gameObject.GetComponent<PlayerComponent>().character;
            if(character == PlayerComponent.PlayerCharacter.Ilio) ilioPresence = false;
            if(character == PlayerComponent.PlayerCharacter.Luna) lunaPresence = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.green;
		if(attachedReactionObject != null) Gizmos.DrawSphere(attachedReactionObject.transform.position, 0.1f);
		if(attachedReactionObject2 != null) Gizmos.DrawSphere(attachedReactionObject2.transform.position, 0.1f);
        //Gizmos.color = Color.white;
        //Gizmos.DrawLine(transform.position, attachedReactionObject.transform.position);
    }

    public void ActivateParticle(ParticleSystem ObjectParticle)
    {        
		Debug.Log("Particle ACTIVE");
        ObjectParticle.Play();
    }
}
