

using System.Collections;
using UnityEngine;

public class EnemyFightState :  EnemyState
{
    private Coroutine playerCheckCoroutine = null;
    private Coroutine playerAttackCoroutine = null;
    private PlayerView playerView;
    public override void OnStateEnter()
    {
     base.OnStateEnter();
     playerView = Model.PlayerTransform.GetComponent<PlayerView>();
     playerCheckCoroutine = StartCoroutine(playerCheck());
        playerAttackCoroutine = StartCoroutine(attackPlayer());
    }
    
    private IEnumerator attackPlayer()
    {
        playerView.TakeDamage(Model.AttackPower);
        yield return new WaitForSeconds(Model.AttackDelay);
        playerAttackCoroutine = StartCoroutine(attackPlayer());

    }


    private IEnumerator playerCheck()
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
}
