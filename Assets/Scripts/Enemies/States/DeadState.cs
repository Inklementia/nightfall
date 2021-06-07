using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState _stateData;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();
    }

    public override void Enter()
    {
        base.Enter();

        _entity.AM.Play("Boom");

        GameObject.Instantiate(_stateData._bloodSplash, _entity.ReturnGroundCheck().position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        GameObject.Instantiate(_stateData._deathBloodParticles, _entity._aliveGo.transform.position, _stateData._deathBloodParticles.transform.rotation);
        GameObject.Instantiate(_stateData._deathChunkParticles, _entity._aliveGo.transform.position, _stateData._deathChunkParticles.transform.rotation);

        _entity.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
