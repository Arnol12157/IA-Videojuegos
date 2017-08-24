using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

    public State initialState;
    public float checkExitRate;
    private State currentState;

    void Awake()
    {
        currentState = initialState;
        currentState.enabled = true;
        InvokeRepeating("Check", 0, checkExitRate);
    }

    //Si en algún momento es destruido, es necesario cancelar la llamada continuada a la función
    void OnDestroy()
    {
        CancelInvoke("Check");
    }

    void Check()
    {
        currentState.CheckExit();
    }

    public void ChangeState(State newState)
    {
        currentState.enabled = false;
        currentState = newState;
        currentState.enabled = true;
    }
}
