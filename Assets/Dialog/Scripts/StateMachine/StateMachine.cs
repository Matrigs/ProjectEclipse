using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class responsible for changing the machine states
public class StateMachine : MonoBehaviour {

    private IState currentlyRunningState;
    private IState PreviousState;

    public void ChangeState(IState newState)
    {
        // check if it's the first time running in the state machine
        if (currentlyRunningState != null) 
        {
            currentlyRunningState.Exit();
        }
        PreviousState = currentlyRunningState;

        currentlyRunningState = newState;
        currentlyRunningState.Enter();
    }

    // Do what the state wants to do
    public void ExecuteStateUpdate()
    {
        if (currentlyRunningState != null)
        {
            currentlyRunningState.Execute();
        }
    }

    public void SwitchToPreviousState()
    {
        currentlyRunningState.Exit();
        currentlyRunningState = PreviousState;
        currentlyRunningState.Enter();
    }
	
}
