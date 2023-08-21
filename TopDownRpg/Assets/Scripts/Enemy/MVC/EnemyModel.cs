

using System.Collections.Generic;
using UnityEngine;
public class EnemyModel 
{
    public int Health;
    public float MovementSpeed;
    public int AttackPower = 10;
    public float AttackDelay = 0.5f;

    public float ObstacelDetectionRadius;
    public float ChaseRadius = 0.8f;
    public float FightRadius = 0.12f;
    public float ColliderSize = 0.08f;
    public float DetectionDelay = 0.5f;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;

    public List<Collider2D> Obstacles = new();
    public Transform PlayerTransform = null;

    public EnemyModel(EnemyScriptableObject scriptableObject)
    {
        this.Health = scriptableObject.Health;
        this.MovementSpeed = scriptableObject.MovementSpeed;
        this.AttackPower = scriptableObject.AttackPower;
        this.AttackDelay = scriptableObject.AttackDelay;
        this.ObstacelDetectionRadius = scriptableObject.ObstacelDetectionRadius;
        this.ChaseRadius = scriptableObject.ChaseRadius;
        this.FightRadius = scriptableObject.FightRadius;
        this.ColliderSize = scriptableObject.ColliderSize;
        this.DetectionDelay = scriptableObject.DetectionDelay;
        this.ObstacleLayerMask = scriptableObject.ObstacleLayerMask;
        this.PlayerLayerMask = scriptableObject.PlayerLayerMask;

    }
}
