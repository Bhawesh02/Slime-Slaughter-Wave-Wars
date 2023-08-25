
using UnityEngine;
[CreateAssetMenu(fileName ="NewPlayer",menuName ="ScriptableObject/NewPlayer")]
public class PlayerScriptableObject : ScriptableObject
{
    public int MaxHealth ;
    public int CurrentHealth ;
    public float MovementSpeed ;
    public float AttackRadius ;
    public float AttackRate ;
    [Header("Attack Point Offset")]
    [Header("Down")]
    public float AttackPointDownYOffset ;
    public float AttackPointDownXOffset;
    [Header("Up")]
    public float AttackPointUpYOffset;
    public float AttackPointUpXOffset = 0f;
    [Header("Right")]

    public float AttackPointRightYOffset;
    public float AttackPointRightXOffset;
    [Header("Left")]

    public float AttackPointLeftYOffset;
    public float AttackPointLeftXOffset;
}
