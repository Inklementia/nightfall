using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    //this script will call animations

    public AttackState _attackState;


    private void TriggerAttack()
    {
        _attackState.TriggerAttack();
    }
    private void FinishAttack()
    {
        _attackState.FinishAttack();
    }
}
