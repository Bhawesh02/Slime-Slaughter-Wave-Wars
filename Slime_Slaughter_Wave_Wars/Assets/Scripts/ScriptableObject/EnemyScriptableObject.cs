
using UnityEngine;

[CreateAssetMenu(fileName ="NewEnemyScriptableObject",menuName ="ScriptableObject/NewEnemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Basic Info")]
    public int MaxHealth;
    public float MovementSpeed;
    public int AttackPower;
    public float AttackDelay;
    [Header("AI Detection Info")]
    public float ObstacelDetectionRadius;
    public float ChaseRadius;
    public float FightRadius;
    public float ColliderSize;
    public float DetectionDelay;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;
    public LayerMask PlayerHideMask;
}
