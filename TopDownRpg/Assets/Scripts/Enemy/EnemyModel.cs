

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public int MovementSpeed;
    public float ObstacelDetectionRadius;
    public float TargetDetectionRadius;
    public LayerMask ObstacleLayerMask;
}
