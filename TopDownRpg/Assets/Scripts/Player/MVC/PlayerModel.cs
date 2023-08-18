
using System;

[Serializable]
public class PlayerModel 
{
    public float MovementSpeed;
    public string[] AnimationDirectionBools = { "IsLookingDown", "IsLookingUp", "IsLookingHrizontal" };
    public LookDirection CurrentLookDirection;
}
