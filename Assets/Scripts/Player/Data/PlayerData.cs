using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float MaxHealth = 100f;
    public float InitialHealth = 30f;

    [Header("Move State")]
    public float MovementVelocity = 10f;

    [Header("Jump State")]
    public float JumpVelocity = 15f;
    public int amountOfJumps = 2;

    [Header("Wall Jump State")]
    public float WallJumpVelocity = 20;
    public float WallJumpTime = 0.4f;
    public Vector2 WallJumpAngle = new Vector2(1, 2);

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity = 2f;

    [Header("Wall Climb State")]
    public float WallClimbVelocity = 3f;

    [Header("Ledge Climb State")]
    public Vector2 StartOffset;
    public Vector2 StopOffset;

    [Header("Dash State")]
    public float DashCooldown = 0.5f;
    public float MaxHoldTime = 1f;
    public float HoldTimeScale = 0.25f; //how much we will slow the time;
    public float DashTime = 0.2f;
    public float DashVelocity = 30;
    public float Drag = 10; //how air density affect object velocity 
    public float DashEndYMultiplier; // when we will end dash, the velocity will be super big, therefore we will multiply it on this value
    public float DistanceBetweenAfterImages = 0.5f;

    [Header("Attack State")]
    public float DamageAmount = 10;
    public float StunDamageAmount = 1;
    public float AttackCooldown = 0.5f;
    public float AttackDrag = 5; //to slow down player whn he attacks 
    public float KnockbackVelocity = 10;

    [Header("Knockback")]
    public float KnockbackDuration = 0.2f;

    [Header("Particles")]
    public GameObject DeathChunkParticles;
    public GameObject DeathBloodParticles;
    public GameObject HitParticles;
    public GameObject BloodSplash;

    [Header("Check Variables")]
    public float GroundCheckRadius = 0.3f;
    public float WallCheckDistance = 0.6f;
    public float AttackHitboxRadius = 0.8f;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsDamageable;


}
