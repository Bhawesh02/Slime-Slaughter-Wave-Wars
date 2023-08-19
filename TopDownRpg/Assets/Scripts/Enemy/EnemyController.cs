
using System;
using UnityEngine;
public class EnemyController 
{
    private EnemyView enemyView;
    private EnemyModel enemyModel;
    private Transform playerTarget;
    private Collider2D[] obstacles = null ;
    public EnemyController(EnemyView enemyView, EnemyModel enemyModel) 
    {
        this.enemyView = enemyView;
        this.enemyModel = enemyModel;
    }
    
    public void Detect()
    {
        obstacles = Physics2D.OverlapCircleAll(enemyView.transform.position,enemyModel.DetectionRadius,enemyModel.ObstacleLayerMask);
    }
    public void DrawDetectionGizmos()
    {

        if(!Application.isPlaying || obstacles == null)
            return;
        Gizmos.color = Color.red;
        foreach (Collider2D col in obstacles)
        {
            Gizmos.DrawSphere(col.transform.position,0.02f);
        }
    }
    public void ReduceHealth()
    {
        enemyModel.Health -= 10;
        if(enemyModel.Health <= 0 )
        {
            enemyView.GetAnimator.SetTrigger("Died");
        }
        else
        {
            enemyView.GetAnimator.SetTrigger("Attacked");
        }
    }
}
