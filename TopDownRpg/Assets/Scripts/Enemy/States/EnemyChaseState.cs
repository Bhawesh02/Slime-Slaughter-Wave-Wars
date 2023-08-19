
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    #region Gizmos
    public bool ShowGizmo = true;

    private float[] dangerResultTemp = null;
    private float[] intrestResultTemp = null;
    #endregion
    private Vector2 targetPos;

    private float[] danger = new float[8];

    private float[] interset = new float[8];
    private List<Vector2> eightDirection = new List<Vector2> {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,

    };
    public override void OnStateEnter()
    {
        base.OnStateEnter();

    }
    private void Update()
    {
        getObstacelsDanger();
        getTargetIntrest();
    }

    private void getTargetIntrest()
    {
        if (Model.PlayerTarget == null || targetPos == null) return;
        
        targetPos = Model.PlayerTarget.position;
        if (Vector2.Distance(transform.position, targetPos) < Model.TargetReachedThersold)
        {
            View.ChangeState(View.FightState);
            return;
        }
        Vector2 directionToTarget = (targetPos - (Vector2)transform.position).normalized;
        float result;
        for (int i = 0; i < eightDirection.Count; i++)
        {
            result = Vector2.Dot(directionToTarget, eightDirection[i]);
            if (result >= 0)
            {
                interset[i] = result;
                
            }
        }
        intrestResultTemp = interset;


    }

    private void getObstacelsDanger()
    {
        Vector2 directionToObstacle;
        float distanceToObstacle;
        Vector2 directionToObstacleNormalized;
        float weight;
        foreach (Collider2D obstacleCollider in Model.Obstacles)
        {
            directionToObstacle = (Vector2)obstacleCollider.transform.position
                - (Vector2)transform.position;
            distanceToObstacle = directionToObstacle.magnitude;
            directionToObstacleNormalized = directionToObstacle.normalized;
            weight = (distanceToObstacle <= Model.ColliderSize)
                ? 1 :
                (Model.ObstacelDetectionRadius - distanceToObstacle)
                / Model.ObstacelDetectionRadius;
            getDangerFromObstacle(directionToObstacleNormalized, weight);
        }
    }

    private void getDangerFromObstacle(Vector2 directionToObstacleNormalized, float weight)
    {
        float result;
        float valueToPut;
        for (int i = 0; i < eightDirection.Count; i++)
        {
            result = Vector2.Dot(directionToObstacleNormalized, eightDirection[i]);
            valueToPut = result * weight;
            if(valueToPut >= 0)
            danger[i] = valueToPut;
            
        }
        dangerResultTemp = danger;

    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo)
            return;
        if (!Application.isPlaying || dangerResultTemp == null) return;
        Gizmos.DrawSphere(targetPos, 0.02f);

        Gizmos.color = Color.red;
        for (int i = 0; i < eightDirection.Count; i++)
        {
            Gizmos.DrawRay(transform.position, eightDirection[i] * danger[i]);
        }
        if (intrestResultTemp == null) return;
        Gizmos.color = Color.green;
        for (int i = 1; i < intrestResultTemp.Length; i++)
        {
            Gizmos.DrawRay(transform.position, eightDirection[i] * intrestResultTemp[i]);
        }
        if (Model.PlayerTarget == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.01f);
        }

    }
}

