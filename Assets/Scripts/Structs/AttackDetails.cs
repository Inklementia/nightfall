using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails
{
    // to hold attack details (used for enemies, player and DeathZones)
    public Vector2 Position;
    public float DamageAmount;
    public float StunDamageAmount;

    public Vector2 KnockbackAngle;
    public float KnockbackVeclocity;
}
