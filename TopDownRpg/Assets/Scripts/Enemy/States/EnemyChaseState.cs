
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public bool ShowGizmo = true;


    private Vector2 targetPos;

    private float[] danger;

    private float[] interest;
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

    private Vector2 resultDirection = Vector2.zero;
    private float nextSearchTime;

    protected override void Awake()
    {
        base.Awake();
        danger = new float[eightDirection.Count];
        interest = new float[eightDirection.Count];
    }
    private void Start()
    {
        nextSearchTime = Time.time;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (Model == null)
        {
            Debug.LogError("Model Null");
        }
        targetPos = Model.PlayerTarget.position;
    }
    private void Update()
    {
        if (Time.time < nextSearchTime)
            return;
        if (targetPos == null)
        {
            View.ChangeState(View.IdelState);
            return;
        }
        if (Vector2.Distance(transform.position, targetPos) < Model.TargetReachedThersold)
        {
            if (Model.PlayerTarget != null)
                View.ChangeState(View.FightState);
            else
                View.ChangeState(View.IdelState);
            return;
        }
        aiToMove();
        View.GetRigidbody.velocity = resultDirection * Model.MovementSpeed;
        nextSearchTime = Time.time + Model.DetectionDelay;
    }

    private void aiToMove()
    {

        Controller.DetectObstacels();
        if (Model.PlayerTarget != null)
            Controller.CheckIfPlayerIsInSight();
        getObstacelsDanger();
        getTargetIntrest();
        getDirectionToMove();
    }

    private void getDirectionToMove()
    {
        for (int i = 0; i < interest.Length; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }
        for (int i = 0; i < interest.Length; i++)
            resultDirection += eightDirection[i] * interest[i];
        resultDirection.Normalize();


    }

    private void getTargetIntrest()
    {
        if (Model.PlayerTarget != null)
            targetPos = Model.PlayerTarget.position;
        Vector2 directionToTarget = (targetPos - (Vector2)transform.position).normalized;
        float result;
        for (int i = 0; i < eightDirection.Count; i++)
        {
            result = Vector2.Dot(directionToTarget, eightDirection[i]);
            if (result >= 0)
            {
                interest[i] = result;

            }
        }


    }

    private void getObstacelsDanger()
    {
        if (Model.Obstacles == null) return;
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
            if (valueToPut >= 0)
                danger[i] = valueToPut;

        }

    }

    public override void OnStateExit()
    {
        View.GetRigidbody.velocity = Vector2.zero;
        base.OnStateExit();
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo)
            return;
        if (!Application.isPlaying || interest == null) return;
        Gizmos.DrawSphere(targetPos, 0.02f);


        if (interest == null) return;
        Gizmos.color = Color.green;
        for (int i = 1; i < interest.Length; i++)
        {
            Gizmos.DrawRay(transform.position, eightDirection[i] * interest[i]);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, resultDirection * 0.5f);

        if (Model != null && Model.PlayerTarget == null && targetPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.01f);
        }

    }
}

