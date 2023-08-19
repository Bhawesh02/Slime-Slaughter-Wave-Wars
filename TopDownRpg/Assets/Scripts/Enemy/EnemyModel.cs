

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public int MovementSpeed;
    public float ObstacelDetectionRadius;
    public float TargetDetectionRadius;
    public float RayCasrOffset = 0.02f;
    public LayerMask ObstacleLayerMask;
    public LayerMask PlayerLayerMask;
}
