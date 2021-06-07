using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine _stateMachine;
    protected Entity _entity;

    public float _startTime { get; protected set; }

    protected string _animBoolName;

    //constructor
    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        _entity = entity;
        _stateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        _startTime = Time.time;
        _entity._anim.SetBool(_animBoolName, true);
        DoSurroundingChecks();
    }

    public virtual void Exit()
    {
        _entity._anim.SetBool(_animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoSurroundingChecks();
    }
    public virtual void DoSurroundingChecks()
    {

    }

}
