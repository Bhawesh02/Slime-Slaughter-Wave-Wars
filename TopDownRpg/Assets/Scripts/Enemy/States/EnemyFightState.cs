

using System.Collections;
using UnityEngine;

public class EnemyFightState :  EnemyState
{
    private Coroutine playerCheckCoroutine = null;
    private Coroutine playerAttackCoroutine = null;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerCheckCoroutine = StartCoroutine(playerCheck());

    }
    
    IEnumerator playerCheck()
    {
        if (Model.PlayerTransform == null || Vector2.Distance(transform.position, Model.PlayerTransform.position) > Model.FightRadius)
            View.ChangeState(View.IdelState);
        yield return new WaitForSeconds(Model.DetectionDelay);
        playerCheckCoroutine = StartCoroutine(playerCheck());

    }
    
    private void asyncCleanup()
    {
        if(playerCheckCoroutine != null)
        StopCoroutine(playerCheckCoroutine);
        if(playerAttackCoroutine != null)
        StopCoroutine(playerAttackCoroutine);
    }
    public override void OnStateExit()
    {
        asyncCleanup();
        base.OnStateExit();
    }
    private void OnDestroy()
    {
        asyncCleanup();
    }
}
