
using System;
using UnityEngine;

[Serializable]
public class PlayerModel 
{
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
    public float MovementSpeed = 500f;
    public string[] AnimationDirectionBools = { "IsLookingDown", "IsLookingUp", "IsLookingHorizontal" };
    public LookDirection CurrentLookDirection;
    public float SwingRate = 0.25f;
    public float AttackRadius = 0.5f;
    [Header("Attack Point Offset")]
    [Header("Down")]
    public float AttackPointDownYOffset = 0.17f;
    public float AttackPointDownXOffset = 0f;
    [Header("Up")]
    public float AttackPointUpYOffset = 0.17f;
    public float AttackPointUpXOffset = 0f;
    [Header("Right")]

    public float AttackPointRightYOffset = 0.17f;
    public float AttackPointRightXOffset = 0f;
    [Header("Left")]

    public float AttackPointLeftYOffset = 0.17f;
    public float AttackPointLeftXOffset = 0f;
}
