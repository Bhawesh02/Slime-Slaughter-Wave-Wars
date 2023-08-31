

using System.Collections.Generic;
using UnityEngine;
public class EnemyModel 
{
    public int MaxHealth { get; }
    public int CurrHealth;
    public float MovementSpeed { get; }
    public int AttackPower { get; }
    public float AttackDelay { get; }

    public float ObstacelDetectionRadius { get; }
    public float ChaseRadius { get; }
    public float FightRadius { get; }
    public float ColliderSize { get; }
    public float DetectionDelay { get; }
    public LayerMask ObstacleLayerMask { get; }
    public LayerMask PlayerLayerMask { get; }
    public LayerMask PlayerHideMask { get; }

    public EnemyState CurrentState;


    public List<Collider2D> Obstacles = new();
    public Transform PlayerTransform = null;

    public EnemyModel(EnemyScriptableObject scriptableObject)
    {
        MaxHealth = scriptableObject.MaxHealth;
        MovementSpeed = scriptableObject.MovementSpeed;
        AttackPower = scriptableObject.AttackPower;
        AttackDelay = scriptableObject.AttackDelay;
        ObstacelDetectionRadius = scriptableObject.ObstacelDetectionRadius;
        ChaseRadius = scriptableObject.ChaseRadius;
        FightRadius = scriptableObject.FightRadius;
        ColliderSize = scriptableObject.ColliderSize;
        DetectionDelay = scriptableObject.DetectionDelay;
        ObstacleLayerMask = scriptableObject.ObstacleLayerMask;
        PlayerLayerMask = scriptableObject.PlayerLayerMask;
        PlayerHideMask = scriptableObject.PlayerHideMask;
    }
}
