using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class D_MeleeAttackState : ScriptableObject
{
    public float _attackRadius = 0.5f;
    public float _attackDamage = 10f;

    public Vector2 _knockbackAngle = new Vector2(10,5);
    public float _knockbackVeclocity = 15f;

    public LayerMask _whatIsPlayer;
}
