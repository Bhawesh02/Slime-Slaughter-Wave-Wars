

using UnityEngine;
public class EnemyController
{
    private EnemyView view;
    public EnemyModel Model { get; private set; }
    private Collider2D[] obstacles;

    public EnemyController(EnemyView enemyView, EnemyScriptableObject enemyScriptableObject)
    {
        view = enemyView;
        Model = new(enemyScriptableObject);
        
    }
    
    public void DetectObstacels()
    {
       obstacles = Physics2D.OverlapCircleAll(view.transform.position, Model.ObstacelDetectionRadius, Model.ObstacleLayerMask);
       foreach(Collider2D obs in obstacles)
        {
            if(obs.GetComponent<EnemyView>() != view)
                Model.Obstacles.Add(obs);
        }
    }

    public void PlayerDetect()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(view.transform.position, Model.ChaseRadius, Model.PlayerLayerMask);
        if (playerCollider == null)
        {
            Model.PlayerTransform = null;
            return;
        }
        Model.PlayerTransform = playerCollider.transform;
    }

    public void CheckIfPlayerIsInSight()
    {
        
        Vector2 direction = (Model.PlayerTransform.position - view.transform.position).normalized;
        Vector2 position = (Vector2)(view.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, Model.ChaseRadius,Model.PlayerHideMask);

        if (hit.collider != null && hit.collider.gameObject != Model.PlayerTransform.gameObject)
        {

            Model.PlayerTransform = null;

            return;
        }
    }



    public void DrawDetectionGizmos()
    {
        if (!Application.isPlaying)
            return;
        Gizmos.color = Color.green;
        if (Model.PlayerTransform != null)
            Gizmos.DrawSphere(Model.PlayerTransform.position, 0.02f);
        if (Model.Obstacles == null)
            return;
        Gizmos.color = Color.red;
        foreach (Collider2D col in Model.Obstacles)
        {
            Gizmos.DrawSphere(col.transform.position, 0.02f);
        }

    }
    public void ReduceHealth()
    {
        Model.CurrHealth -= 10;
        if (Model.CurrHealth <= 0)
        {
            view.GetAnimator.SetTrigger("Died");
        }
        else
        {
            view.GetAnimator.SetTrigger("Attacked");
        }
    }

    public void ResetHealth()
    {
        Model.CurrHealth = Model.MaxHealth;
    }

    public void ChangeState(EnemyState state)
    {
        Model.CurrentState?.OnStateExit();
        Model.CurrentState = state;
        Model.CurrentState.OnStateEnter();
    }
    public void EnemyDied()
    {
        EnemyPoolService.Instance.ReturnEnemy(view);
        EventService.Instance.InvokeEnemyDied(view);
        view.StopCoroutine(view.PlayerDetectCoroutine);
        Model.CurrentState?.OnStateExit();
    }
}
