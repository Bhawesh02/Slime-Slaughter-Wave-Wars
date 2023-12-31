
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public bool ShowDangerGizmo = true;
    public bool ShowIntrestGizmo = true;
    public bool ShowPathGizmo = true;


    private Vector2 targetPos;

    private float[] danger;

    private float[] interest;
    private readonly Vector2[] eightDirection = {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,

    };

    private Vector2 resultDirection;

    private Coroutine aiCoroutine;

    protected override void Awake()
    {
        base.Awake();
        danger = new float[eightDirection.Length];
        interest = new float[eightDirection.Length];
    }
    
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (Model == null || Controller == null)
            SetModelController();
        SetVariableToZero();
        targetPos = Model.PlayerTransform.position;
        if (targetPos == null)
        {
            Controller.ChangeState(View.IdelState);
        }
        aiCoroutine = StartCoroutine(AiLogic());
        View.GetAnimator.SetBool("IsMoving", true);
    }

    private void SetVariableToZero()
    {
        for (int i = 0; i < danger.Length; i++)
        {
            danger[i] = interest[i] = 0;
        }
        resultDirection = Vector2.zero;
    }

    private IEnumerator AiLogic()
    {
        

        Controller.DetectObstacels();
        if (Model.PlayerTransform != null)
            Controller.CheckIfPlayerIsInSight();

        if (Vector2.Distance(transform.position, targetPos) <= Model.FightRadius)
        {
            if (Model.PlayerTransform != null)
                Controller.ChangeState(View.FightState);
            else
                Controller.ChangeState(View.IdelState);
            yield break;
        }

        AiToMove();
        View.GetRigidbody.velocity = resultDirection * Model.MovementSpeed;
        if(View.GetRigidbody.velocity.x > 0)
            View.GetSpriteRenderer.flipX = false;
        else
            View.GetSpriteRenderer.flipX = true;
        yield return new WaitForSeconds(Model.DetectionDelay);
        aiCoroutine = StartCoroutine(AiLogic());

    }
    private void AiToMove()
    {

        
        GetObstacelsDanger();
        GetTargetIntrest();
        GetDirectionToMove();
    }

    

    private void GetObstacelsDanger()
    {
        if (Model.Obstacles == null) return;
        Vector2 directionToObstacle;
        float distanceToObstacle;
        Vector2 directionToObstacleNormalized;
        float weight;
        foreach (Collider2D obstacleCollider in Model.Obstacles)
        {
            directionToObstacle = obstacleCollider.ClosestPoint(transform.position)
                - (Vector2)transform.position;
           /* directionToObstacle = (Vector2)obstacleCollider.transform.position
                - (Vector2)transform.position;*/
            distanceToObstacle = directionToObstacle.magnitude;
            directionToObstacleNormalized = directionToObstacle.normalized;
            weight = (distanceToObstacle <= Model.ColliderSize)
                ? 1 :
                (Model.ObstacelDetectionRadius - distanceToObstacle)
                / Model.ObstacelDetectionRadius;
            GetDangerFromObstacle(directionToObstacleNormalized, weight);
        }
    }
    
    private void GetDangerFromObstacle(Vector2 directionToObstacleNormalized, float weight)
    {
        float result;
        float valueToPut;
        for (int i = 0; i < eightDirection.Length; i++)
        {
            result = Vector2.Dot(directionToObstacleNormalized, eightDirection[i]);
            valueToPut = result * weight;
            if (valueToPut >= 0)
                danger[i] = valueToPut;

        }

    }
    private void GetTargetIntrest()
    {
        if (Model.PlayerTransform != null)
            targetPos = Model.PlayerTransform.position;
        Vector2 directionToTarget = (targetPos - (Vector2)transform.position).normalized;
        float result;
        for (int i = 0; i < eightDirection.Length; i++)
        {
            result = Vector2.Dot(directionToTarget, eightDirection[i]);
            if (result >= 0)
            {
                interest[i] = result;

            }
        }


    }
    private void GetDirectionToMove()
    {
        for (int i = 0; i < interest.Length; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }
        for (int i = 0; i < interest.Length; i++)
            resultDirection += eightDirection[i] * interest[i];
        resultDirection.Normalize();


    }

    public override void OnStateExit()
    {
        StopCoroutine(AiLogic());
        View.GetRigidbody.velocity = Vector2.zero;
        View.GetAnimator.SetBool("IsMoving", false);
        SetVariableToZero();
        base.OnStateExit();

    }
   

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || interest == null) return;
        Gizmos.DrawSphere(targetPos, 0.02f);
        if (danger != null && ShowDangerGizmo)
        {
            Gizmos.color = Color.red;
            for (int i = 1; i < interest.Length; i++)
            {
                Gizmos.DrawRay(transform.position, eightDirection[i] * danger[i]);
            }
        }
        if (interest == null) return;
        if (ShowIntrestGizmo)
        {
            Gizmos.color = Color.green;
            for (int i = 1; i < interest.Length; i++)
            {
                Gizmos.DrawRay(transform.position, eightDirection[i] * interest[i]);
            }
        }
        if (!ShowPathGizmo)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, resultDirection * 0.5f);

        if (Model != null && Model.PlayerTransform == null && targetPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.01f);
        }

    }
}

