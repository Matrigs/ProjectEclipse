using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ButtonComponent : InteractiveComponent
{
    public ReactiveComponent attachedReactionObject;
    public buttonState currentState = buttonState.active;

    [SerializeField]
    private bool playerPresence;
    [SerializeField]
    private List<ButtonMaterial> ButtonMaterials = new List<ButtonMaterial>(2);

    [System.Serializable]
    public struct ButtonMaterial
    {
        public buttonState state;
        public Material material;
    }
    public enum buttonState {active, inactive}
    
    private void OnEnable()
    {
        PlayerComponent.ActionButton += Action;
    }
    private void OnDisable()
    {
        PlayerComponent.ActionButton -= Action;
    }

    public override void Action()
    {
        //base.Action();
        if (playerPresence && currentState == buttonState.active)
        {
            Debug.Log("DoorOpen");
            ChangeState();
            attachedReactionObject.Reaction();
        }
    }
    void ChangeState()
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hero");
        if (collision.CompareTag("Player"))
        {
            playerPresence = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPresence = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(attachedReactionObject.transform.position, 0.1f);
        //Gizmos.color = Color.white;
        //Gizmos.DrawLine(transform.position, attachedReactionObject.transform.position);
    }
}
