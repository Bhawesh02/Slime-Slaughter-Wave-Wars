using System.Collections;
using UnityEngine;

public class EnemyIdelState : EnemyState
{
    private Coroutine spriteFlip;
    private readonly float flipMaxTime = 2f;
    private int flipValue;
    private float waitTime;
    private Coroutine checkForPlayer;
   
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (Model == null || Controller == null)
            SetModelController();
        spriteFlip = StartCoroutine(FlipTheSprite());

        checkForPlayer = StartCoroutine(CheckPlayer());
        
    }
    
    private IEnumerator CheckPlayer()
    {
        yield return new WaitForSeconds(Model.DetectionDelay);
        
        if(Model.PlayerTransform != null)
        {
            Controller.ChangeState(View.ChaseState);
            yield break;
        }
        checkForPlayer = StartCoroutine(CheckPlayer());
    }
    private IEnumerator FlipTheSprite()
    {
        flipValue = Random.Range(0, 2);
        if(flipValue == 0)
            View.GetSpriteRenderer.flipX = false;
        else
            View.GetSpriteRenderer.flipX = true;
        waitTime = Random.Range(0f, flipMaxTime);
        yield return new WaitForSeconds(waitTime);
        spriteFlip = StartCoroutine(FlipTheSprite());
    }

    public override void OnStateExit()
    {
        StopCoroutine(spriteFlip);
        base.OnStateExit();
    }
}
