
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyController
{
    private EnemyView enemyView;
    private EnemyModel enemyModel;
    private Transform playerTarget = null;
    private List<Collider2D> obstacles = new();
    public EnemyController(EnemyView enemyView, EnemyModel enemyModel)
    {
        this.enemyView = enemyView;
        this.enemyModel = enemyModel;
    }

    public void DetectObstacelsAndPlayer()
    {
        ObstacelDetect();
        PlayerDetect();
    }

    private void PlayerDetect()
    {
        Collider2D playerCollider = null;
        Collider2D[] targetHits = Physics2D.OverlapCircleAll(enemyView.transform.position, enemyModel.TargetDetectionRadius);
        for (int i = 0; i < targetHits.Length; i++)
        {
            if (targetHits[i].GetComponent<PlayerView>() != null)
            {
                playerCollider = targetHits[i];
                break;
            }
        }
       if(playerCollider == null)
        {
            playerTarget = null;
            return;
        }
        playerTarget = playerCollider.transform;
    }

    private void ObstacelDetect()
    {
        Collider2D[] obstacleTargetHit = Physics2D.OverlapCircleAll(enemyView.transform.position, enemyModel.ObstacelDetectionRadius);
        List<Collider2D> tempObstacels = new();
        for (int i = 0; i < obstacleTargetHit.Length; i++)
        {
            if (obstacleTargetHit[i].GetComponent<EnemyView>() != enemyView && obstacleTargetHit[i].GetComponent<PlayerView>() == null && obstacleTargetHit[i].gameObject.layer != LayerMask.GetMask("Boundry"))
            {
                tempObstacels.Add(obstacleTargetHit[i]);
            }
        }
        obstacles = tempObstacels;
    }

    public void DrawDetectionGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.color = Color.green;
        if (playerTarget != null)
            Gizmos.DrawSphere(playerTarget.position, 0.02f);
        if (obstacles.Count == 0)
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
