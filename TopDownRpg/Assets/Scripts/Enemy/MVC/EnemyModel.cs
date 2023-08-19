

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public int MovementSpeed;
    public float ObstacelDetectionRadius;
    public float TargetDetectionRadius;
    public float TargetReachedThersold = 0.5f;
    public float ColliderSize = 0.08f;
    public float DetectionDelay = 0.5f;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;
    [HideInInspector]
    public Collider2D[] Obstacles = null;
    public Transform PlayerTarget = null;
}
