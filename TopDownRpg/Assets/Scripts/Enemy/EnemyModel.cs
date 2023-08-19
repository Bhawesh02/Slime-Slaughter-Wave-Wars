

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public int MovementSpeed;
    public float ObstacelDetectionRadius;
    public float TargetDetectionRadius;
    public float RayCastOffset = 0.1f;
    public float DetectionDelay = 0.5f;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;
}
