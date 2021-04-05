using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    ProtoOtter otter;
    State currentState;

    private void Start()
    {
        otter = GetComponent<ProtoOtter>();
    }

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine() {
        State nextState = currentState?.RunCurrentState(otter);

        if (nextState != null) {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State nextState) {
        currentState = nextState;
    }
}
