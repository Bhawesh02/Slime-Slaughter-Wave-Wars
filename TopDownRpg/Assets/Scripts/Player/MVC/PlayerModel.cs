
using System;

[Serializable]
public class PlayerModel 
{
    public int MaxHealth {get;private set;}
    public int CurrentHealth ;
    public float MovementSpeed { get; private set; }
    public PlayerAnimationStates CurrentAnimation;
    public LookDirection CurrentLookDirection;
    public float AttackRate { get; private set; }
    public float AttackRadius { get; private set; }
    public float AttackPointDownYOffset { get; private set; }
    public float AttackPointDownXOffset { get; private set; }
    public float AttackPointUpYOffset { get; private set; }
    public float AttackPointUpXOffset { get; private set; }
    public float AttackPointRightYOffset { get; private set; }
    public float AttackPointRightXOffset { get; private set; }
    public float AttackPointLeftYOffset { get; private set; }
    public float AttackPointLeftXOffset { get; private set; }

    public PlayerModel(PlayerScriptableObject scriptableObject) 
    {
        this.MaxHealth = scriptableObject.MaxHealth;
        this.CurrentHealth = scriptableObject.CurrentHealth;
        this.MovementSpeed = scriptableObject.MovementSpeed;
        this.AttackRadius = scriptableObject.AttackRadius;
        this.AttackRate = scriptableObject.AttackRate;
        this.AttackPointDownYOffset = scriptableObject.AttackPointDownYOffset;
        this.AttackPointDownXOffset = scriptableObject.AttackPointDownXOffset;
        this.AttackPointUpYOffset = scriptableObject.AttackPointUpYOffset;
        this.AttackPointUpXOffset = scriptableObject.AttackPointUpXOffset;
        this.AttackPointRightYOffset = scriptableObject.AttackPointRightYOffset;
        this.AttackPointRightXOffset = scriptableObject.AttackPointRightXOffset;
        this.AttackPointLeftYOffset = scriptableObject.AttackPointLeftYOffset; 
        this.AttackPointLeftXOffset = scriptableObject.AttackPointLeftXOffset;
    }
}
