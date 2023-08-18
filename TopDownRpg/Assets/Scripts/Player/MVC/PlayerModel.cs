
using System;

[Serializable]
public class PlayerModel 
{
    public float MovementSpeed = 500f;
    public string[] AnimationDirectionBools = { "IsLookingDown", "IsLookingUp", "IsLookingHrizontal" };
    public LookDirection CurrentLookDirection;
    public float SwingRate = 0.25f;
    public float AttackRadius = 0.5f;
}
