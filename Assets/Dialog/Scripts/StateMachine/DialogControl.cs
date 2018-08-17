using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogControl : MonoBehaviour {

    private StateMachine stateMachine = new StateMachine();

    private void Start()
    {
       
    }

    public void showSpeechBallon(GameObject target)
    {
        Begin beginState = this.GetComponent<Begin>();
        beginState.Target = target;
        this.stateMachine.ChangeState(beginState);
        this.stateMachine.ExecuteStateUpdate();
    }

    private void Update()
    {

    }


}
