

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public float MovementSpeed;
    public float ObstacelDetectionRadius;
    public float ChaseRadius = 0.8f;
    public float FightRadius = 0.12f;
    public float ColliderSize = 0.08f;
    public float DetectionDelay = 0.5f;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;
    [HideInInspector]
    public Collider2D[] Obstacles = null;
    public Transform PlayerTransform = null;
}
