using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {

    // initial function of the state
    void Enter();

    // what this state continualy does when active
    void Execute();

    // final action of the state before leaving
    void Exit();



	
}
