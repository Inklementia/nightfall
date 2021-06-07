using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Range Attack State")]
public class D_RangedAttackState : ScriptableObject
{
    public GameObject _projectile;

    public float _projectileDamage = 10;
    public float _projectileSpeed = 12;

    public Vector2 _knockbackAngle = new Vector2(5, 2);
    public float _knockbackVeclocity = 10f;

    public float _projectileTravelDistance; //how much projectile should fly until it affected by gravity
}
