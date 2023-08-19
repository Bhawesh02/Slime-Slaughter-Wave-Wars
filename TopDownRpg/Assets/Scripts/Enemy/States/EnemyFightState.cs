

using UnityEngine;

public class EnemyFightState :  EnemyState
{
    private float nextDetectionTime;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

    }
    private void Start()
    {
        nextDetectionTime = Time.time;
    }
    private void Update()
    {
        if( Time.time < nextDetectionTime) 
        {
            return;
        }
        if (Model.PlayerTarget == null || Vector2.Distance(transform.position, Model.PlayerTarget.position) > Model.TargetReachedThersold)
            View.ChangeState(View.IdelState);
        nextDetectionTime = Time.time + Model.DetectionDelay;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
