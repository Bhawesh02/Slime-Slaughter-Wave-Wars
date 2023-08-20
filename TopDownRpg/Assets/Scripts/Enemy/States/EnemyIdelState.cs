using System.Collections;
using UnityEngine;

public class EnemyIdelState : EnemyState
{
    private Coroutine spriteFlip;
    private readonly float flipMaxTime = 2f;
    private int flipValue;
    private float waitTime;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        spriteFlip = StartCoroutine(FlipTheSprite());
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
