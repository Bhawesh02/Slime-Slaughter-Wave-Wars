

using System.Collections;
using UnityEngine;

public class EnemyFightState :  EnemyState
{
    private Coroutine playerCheckCoroutine = null;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerCheckCoroutine = StartCoroutine(playerCheck());

    }
    
    IEnumerator playerCheck()
    {
        if (Model.PlayerTarget == null || Vector2.Distance(transform.position, Model.PlayerTarget.position) > Model.TargetReachedThersold)
            View.ChangeState(View.IdelState);
        yield return new WaitForSeconds(Model.DetectionDelay);
        playerCheckCoroutine = StartCoroutine(playerCheck());

    }
    
    public override void OnStateExit()
    {
        StopCoroutine(playerCheckCoroutine);
        base.OnStateExit();
    }
}
