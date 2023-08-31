
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerController
{
    public PlayerModel Model { get; }
    private readonly PlayerView view;
    private float newXPosForAttackPoint;
    private float newYPosForAttackPoint;
    private Vector2 movementDirection;
    private CancellationTokenSource cancellationTokenSource = new();
    private bool isAttacking = false;
    public PlayerController(PlayerScriptableObject playerScriptableObject, PlayerView playerView)
    {
        Model = new(playerScriptableObject);
        view = playerView;
    }

    public void ChangeState(PlayerState playerState)
    {
        view.CurrentState?.OnStateExit();
        view.CurrentState = playerState;
        view.CurrentState.OnStateEnter();
    }

    public void MovePlayer()
    {
        movementDirection = view.MoveVector;
        movementDirection.Normalize();
        view.PlayerRigidBody.velocity = Model.MovementSpeed * Time.deltaTime * movementDirection;
    }
    public void SetLookAndAttackPointDirection(LookDirection direction)
    {
        if (direction == Model.CurrentLookDirection)
            return;
        switch (direction)
        {
            case LookDirection.Down:
                newXPosForAttackPoint = view.transform.position.x + Model.AttackPointDownXOffset;
                newYPosForAttackPoint = view.transform.position.y + Model.AttackPointDownYOffset + view.AttackPointInitialYOffset;
                break;
            case LookDirection.Up:
                newXPosForAttackPoint = view.transform.position.x + Model.AttackPointUpXOffset;
                newYPosForAttackPoint = view.transform.position.y + Model.AttackPointUpYOffset + view.AttackPointInitialYOffset;


                break;
            case LookDirection.Left:
                newXPosForAttackPoint = view.transform.position.x + Model.AttackPointLeftXOffset;
                newYPosForAttackPoint = view.transform.position.y + Model.AttackPointLeftYOffset + view.AttackPointInitialYOffset;


                view.PlayerSpriteRenderer.flipX = true;
                break;
            case LookDirection.Right:
                newXPosForAttackPoint = view.transform.position.x + Model.AttackPointRightXOffset;
                newYPosForAttackPoint = view.transform.position.y + Model.AttackPointRightYOffset + view.AttackPointInitialYOffset;

                view.PlayerSpriteRenderer.flipX = false;
                break;
        }
        view.AttackPoint.position = new(newXPosForAttackPoint, newYPosForAttackPoint);
        Model.CurrentLookDirection = direction;
    }

    #region Player Animation Change

    private float GetAnimationClipLength(PlayerAnimationStates animation)
    {
        Animator anim = view.PlayerAnimator;
        string clipName = animation.ToString();
        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogError("Animation clip not found: " + clipName);
        return 0f;
    }
    public void ChangePlayerAnimation(PlayerAnimationStates animation)
    {
        if (isAttacking||animation == Model.CurrentAnimation||Model.CurrentAnimation == PlayerAnimationStates.Dead)
            return;
        
        view.PlayerAnimator.Play(animation.ToString());
        Model.CurrentAnimation = animation;
    }

    private void PlayPlayerFightAnimation()
    {
        switch (Model.CurrentLookDirection)
        {

            case LookDirection.Down:
                ChangePlayerAnimation(PlayerAnimationStates.Down_Fight);
                break;
            case LookDirection.Up:
                ChangePlayerAnimation(PlayerAnimationStates.Up_Fight);

                break;
            case LookDirection.Left:
            case LookDirection.Right:
                ChangePlayerAnimation(PlayerAnimationStates.Horizontal_Fight);
                break;
        }
    }

    public void PlayPlayerIdelAnimation()
    {
        switch (Model.CurrentLookDirection)
        {
            case LookDirection.Down:
                ChangePlayerAnimation(PlayerAnimationStates.Down_Idel);
                break;
            case LookDirection.Up:
                ChangePlayerAnimation(PlayerAnimationStates.Up_Idel);
                break;
            case LookDirection.Left:
            case LookDirection.Right:
                ChangePlayerAnimation(PlayerAnimationStates.Horizontal_Idel);
                break;
        }
    }

    public void PlayPlayerRunningAnimation()
    {
        switch (Model.CurrentLookDirection)
        {
            case LookDirection.Down:
                ChangePlayerAnimation(PlayerAnimationStates.Down_Running);
                break;
            case LookDirection.Up:
                ChangePlayerAnimation(PlayerAnimationStates.Up_Running);
                break;
            case LookDirection.Left:
            case LookDirection.Right:
                ChangePlayerAnimation(PlayerAnimationStates.Horizontal_Running);
                break;
        }
    }

    //Can't use invoke since non-monoehaivour script
    private async void ResetPlayerAnimation()
    {
        try
        {
            await Task.Delay((int)(1000f * GetAnimationClipLength(Model.CurrentAnimation)), cancellationTokenSource.Token);
            isAttacking = false;
            if (view.CurrentState == view.PlayerIdelState)
                PlayPlayerIdelAnimation();
            else
                PlayPlayerRunningAnimation();
        }
        catch (TaskCanceledException)
        {
        }
    }
    #endregion
    public void PlayerAttack()
    {
        PlayPlayerFightAnimation();
        isAttacking = true;
        SoundService.Instance.PlaySfx(SoundService.Instance.Slash);
        Collider2D[] hits = Physics2D.OverlapCircleAll(view.AttackPoint.position, Model.AttackRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponent<IDamageable>()?.TakeDamage();
        }

        ResetPlayerAnimation();
    }
    public void ReduceHealth(int attackPower)
    {
        ChangePlayerAnimation(PlayerAnimationStates.Took_Damage);
        SoundService.Instance.PlaySfx(SoundService.Instance.Hurt);
        Model.CurrentHealth -= attackPower;
        if (Model.CurrentHealth <= 0)
        {
            PlayerDead();
            return;
        }
        ResetPlayerAnimation();

    }




    private void PlayerDead()
    {
        cancellationTokenSource?.Cancel();
        ChangePlayerAnimation(PlayerAnimationStates.Dead);
        view.enabled = false;
        GameManager.Instance.PlayedDied();
    }

   
}
