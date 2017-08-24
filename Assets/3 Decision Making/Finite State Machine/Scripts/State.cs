using UnityEngine;
using System.Collections;

[RequireComponent(typeof(StateMachine))]
public class State : MonoBehaviour
{
    protected StateMachine stateMachine;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public virtual void CheckExit()
	{
		
	}
}
