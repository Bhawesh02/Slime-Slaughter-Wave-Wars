
using System;

[Serializable]
public class PlayerModel 
{
    public int MaxHealth { get; }
    public int CurrentHealth ;
    public float MovementSpeed { get; }
    public PlayerAnimationStates CurrentAnimation;
    public LookDirection CurrentLookDirection;
    public float AttackRate { get;  }
    public float AttackRadius { get;  }
    public float AttackPointDownYOffset { get; }
    public float AttackPointDownXOffset { get; }
    public float AttackPointUpYOffset { get; }
    public float AttackPointUpXOffset { get; }
    public float AttackPointRightYOffset { get; }
    public float AttackPointRightXOffset { get; }
    public float AttackPointLeftYOffset { get; }
    public float AttackPointLeftXOffset { get; }

    public PlayerModel(PlayerScriptableObject scriptableObject) 
    {
        MaxHealth = scriptableObject.MaxHealth;
        CurrentHealth = scriptableObject.CurrentHealth;
        MovementSpeed = scriptableObject.MovementSpeed;
        AttackRadius = scriptableObject.AttackRadius;
        AttackRate = scriptableObject.AttackRate;
        AttackPointDownYOffset = scriptableObject.AttackPointDownYOffset;
        AttackPointDownXOffset = scriptableObject.AttackPointDownXOffset;
        AttackPointUpYOffset = scriptableObject.AttackPointUpYOffset;
        AttackPointUpXOffset = scriptableObject.AttackPointUpXOffset;
        AttackPointRightYOffset = scriptableObject.AttackPointRightYOffset;
        AttackPointRightXOffset = scriptableObject.AttackPointRightXOffset;
        AttackPointLeftYOffset = scriptableObject.AttackPointLeftYOffset; 
        AttackPointLeftXOffset = scriptableObject.AttackPointLeftXOffset;
    }
}
