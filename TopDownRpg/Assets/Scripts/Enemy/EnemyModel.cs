

using System;
using UnityEngine;

[Serializable]
public class EnemyModel 
{
    public int Health;
    public int MovementSpeed;
    public float DetectionRadius;
    public LayerMask ObstacleLayerMask;
}
