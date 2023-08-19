
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyController
{
    private EnemyView enemyView;
    private EnemyModel enemyModel;
    private Transform playerTarget = null;
    private Collider2D[] obstacles = null;
    public EnemyController(EnemyView enemyView, EnemyModel enemyModel)
    {
        this.enemyView = enemyView;
        this.enemyModel = enemyModel;
    }

    public void DetectObstacelsAndPlayer()
    {
        obstacles = Physics2D.OverlapCircleAll(enemyView.transform.position, enemyModel.ObstacelDetectionRadius, enemyModel.ObstacleLayerMask);

        PlayerDetect();
    }

    private void PlayerDetect()
    {
        
        
        Collider2D playerCollider = Physics2D.OverlapCircle(enemyView.transform.position, enemyModel.TargetDetectionRadius, enemyModel.PlayerLayerMask); 

        if (playerCollider == null)
        {
            playerTarget = null;
            return;
        }
        playerTarget = playerCollider.transform;
        CheckIfPlayerIsInSight();
    }

    private void CheckIfPlayerIsInSight()
    {
        Vector2 direction = (playerTarget.position - enemyView.transform.position).normalized;
        Vector2 position = (Vector2)(enemyView.transform.position) + direction * enemyModel.RayCastOffset ;
        RaycastHit2D hit = Physics2D.Raycast(position, direction,enemyModel.TargetDetectionRadius);
        
        if(hit.collider.gameObject != playerTarget.gameObject)
        {
            playerTarget = null;
            return;
        }
    }

    

    public void DrawDetectionGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.color = Color.green;
        if (playerTarget != null)
            Gizmos.DrawSphere(playerTarget.position, 0.02f);
        if (obstacles == null)
            return;
        Gizmos.color = Color.red;
        foreach (Collider2D col in obstacles)
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
}
