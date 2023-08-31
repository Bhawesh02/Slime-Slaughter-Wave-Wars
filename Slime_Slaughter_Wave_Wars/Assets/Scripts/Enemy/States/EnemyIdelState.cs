using System.Collections;
using UnityEngine;

public class EnemyIdelState : EnemyState
{
    private readonly float flipMaxTime = 2f;
    private int flipValue;
    private float flipWaitTime;
    private float nextCheckForPlayer;

    protected override void Start()
    {
        base.Start();
        nextCheckForPlayer = Time.time;
        flipWaitTime = Time.time;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (Model == null || Controller == null)
            SetModelController();
    }
    
    private void Update()
    {
        if (Time.time >= nextCheckForPlayer)
        {
            if (Model.PlayerTransform != null)
                Controller.ChangeState(View.ChaseState);
            nextCheckForPlayer = Time.time + Model.DetectionDelay;
        }
        if(Time.time >= flipWaitTime)
        {
            flipValue = Random.Range(0, 2);
            if (flipValue == 0)
                View.GetSpriteRenderer.flipX = false;
            else
                View.GetSpriteRenderer.flipX = true;
            flipWaitTime = Time.time + Random.Range(0f, flipMaxTime);
        }
    }
    

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
