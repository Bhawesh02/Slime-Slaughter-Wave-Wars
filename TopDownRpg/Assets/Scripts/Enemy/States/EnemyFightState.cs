

using System.Collections;
using UnityEngine;

public class EnemyFightState : EnemyState
{
    private Coroutine playerCheckCoroutine = null;
    private Coroutine playerAttackCoroutine = null;
    private PlayerView playerView;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        playerView = Model.PlayerTransform.GetComponent<PlayerView>();
        playerCheckCoroutine = StartCoroutine(playerCheck());

        startAttackPlayer();
    }

    private void startAttackPlayer()
    {
        if (playerAttackCoroutine != null)
        {
            return;
        }
        playerAttackCoroutine = StartCoroutine(attackPlayer());
    }

    private IEnumerator attackPlayer()
    {
        if (View.CurrentState != this)
        {
            yield break;
        }

        playerView.TakeDamage(Model.AttackPower);
        yield return new WaitForSeconds(Model.AttackDelay);

        playerAttackCoroutine = StartCoroutine(attackPlayer());

    }


    private IEnumerator playerCheck()
    {
        if (Model.PlayerTransform == null || Vector2.Distance(transform.position, Model.PlayerTransform.position) > Model.FightRadius)
        {

            View.ChangeState(View.IdelState);
            yield break;
        }
        yield return new WaitForSeconds(Model.DetectionDelay);
        playerCheckCoroutine = StartCoroutine(playerCheck());

    }


    public override void OnStateExit()
    {
        if (playerCheckCoroutine != null)
            StopCoroutine(playerCheckCoroutine);
        base.OnStateExit();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
