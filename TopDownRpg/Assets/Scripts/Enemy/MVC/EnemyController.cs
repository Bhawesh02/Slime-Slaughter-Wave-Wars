

using System;
using UnityEngine;
public class EnemyController
{
    private EnemyView enemyView;
    private EnemyModel enemyModel;

    public EnemyController(EnemyView enemyView, EnemyModel enemyModel)
    {
        this.enemyView = enemyView;
        this.enemyModel = enemyModel;
    }

    public void DetectObstacels()
    {
        enemyModel.Obstacles = Physics2D.OverlapCircleAll(enemyView.transform.position, enemyModel.ObstacelDetectionRadius, enemyModel.ObstacleLayerMask);

    }

    public void PlayerDetect()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(enemyView.transform.position, enemyModel.ChaseRadius, enemyModel.PlayerLayerMask);
        if (playerCollider == null)
        {
            enemyModel.PlayerTransform = null;
            return;
        }
        enemyModel.PlayerTransform = playerCollider.transform;
    }

    public void CheckIfPlayerIsInSight()
    {
        
        Vector2 direction = (enemyModel.PlayerTransform.position - enemyView.transform.position).normalized;
        Vector2 position = (Vector2)(enemyView.transform.position) + direction * enemyModel.ColliderSize;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, enemyModel.ChaseRadius,enemyModel.ObstacleLayerMask);

        if (hit.collider != null && hit.collider.gameObject != enemyModel.PlayerTransform.gameObject)
        {
            enemyModel.PlayerTransform = null;
            return;
        }
    }



    public void DrawDetectionGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.color = Color.green;
        if (enemyModel.PlayerTransform != null)
            Gizmos.DrawSphere(enemyModel.PlayerTransform.position, 0.02f);
        if (enemyModel.Obstacles == null)
            return;
        Gizmos.color = Color.red;
        foreach (Collider2D col in enemyModel.Obstacles)
        {
            Gizmos.DrawSphere(col.transform.position, 0.02f);
        }

    }
    public void ReduceHealth()
    {
        enemyModel.Health -= 10;
        if (enemyModel.Health <= 0)
        {
            enemyView.GetAnimator.SetTrigger("Died");
        }
        else
        {
            enemyView.GetAnimator.SetTrigger("Attacked");
        }
    }

    public void ResetHealth(EnemyScriptableObject enemyScriptableObject)
    {
        enemyModel.Health = enemyScriptableObject.Health;
    }
}
