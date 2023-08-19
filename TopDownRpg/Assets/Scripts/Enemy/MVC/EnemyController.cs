

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

    public void DetectObstacelsAndPlayer()
    {
        enemyModel.Obstacles = Physics2D.OverlapCircleAll(enemyView.transform.position, enemyModel.ObstacelDetectionRadius, enemyModel.ObstacleLayerMask);

        PlayerDetect();
    }

    private void PlayerDetect()
    {
        
        
        Collider2D playerCollider = Physics2D.OverlapCircle(enemyView.transform.position, enemyModel.TargetDetectionRadius, enemyModel.PlayerLayerMask); 

        if (playerCollider == null)
        {
            enemyModel.PlayerTarget = null;
            return;
        }
        enemyModel.PlayerTarget = playerCollider.transform;
        CheckIfPlayerIsInSight();
    }

    private void CheckIfPlayerIsInSight()
    {
        Vector2 direction = (enemyModel.PlayerTarget.position - enemyView.transform.position).normalized;
        Vector2 position = (Vector2)(enemyView.transform.position) + direction * enemyModel.ColliderSize ;
        RaycastHit2D hit = Physics2D.Raycast(position, direction,enemyModel.TargetDetectionRadius);
        
        if(hit.collider.gameObject != enemyModel.PlayerTarget.gameObject)
        {
            enemyModel.PlayerTarget = null;
            return;
        }
    }

    

    public void DrawDetectionGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.color = Color.green;
        if (enemyModel.PlayerTarget != null)
            Gizmos.DrawSphere(enemyModel.PlayerTarget.position, 0.02f);
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
}
