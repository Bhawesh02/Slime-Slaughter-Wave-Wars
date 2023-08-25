
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private float newXPosForAttackPoint;
    private float newYPosForAttackPoint;
    private Vector2 movementDirection;
    
    public PlayerController(PlayerModel playerModel, PlayerView playerView)
    {
        this.playerModel = playerModel;
        this.playerView = playerView;
    }

    public void MovePlayer()
    {
        movementDirection = playerView.MoveVector;
        movementDirection.Normalize();
        playerView.PlayerRigidBody.velocity = playerModel.MovementSpeed * Time.deltaTime * movementDirection;
    }
    public void SetLookAndAttackPointDirection(LookDirection direction)
    {
        if (direction == playerModel.CurrentLookDirection)
            return;
        switch (direction)
        {
            case LookDirection.Down:
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointDownXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointDownYOffset + playerView.AttackPointInitialYOffset;
                break;
            case LookDirection.Up:
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointUpXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointUpYOffset + playerView.AttackPointInitialYOffset;
                

                break;
            case LookDirection.Left:
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointLeftXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointLeftYOffset + playerView.AttackPointInitialYOffset;
                

                playerView.PlayerSpriteRenderer.flipX = true;
                break;
            case LookDirection.Right:
                newXPosForAttackPoint = playerView.transform.position.x + playerView.PlayerModel.AttackPointRightXOffset;
                newYPosForAttackPoint = playerView.transform.position.y + playerView.PlayerModel.AttackPointRightYOffset + playerView.AttackPointInitialYOffset;
               
                playerView.PlayerSpriteRenderer.flipX = false;
                break;
        }
        playerView.AttackPoint.position = new(newXPosForAttackPoint,newYPosForAttackPoint);
        playerModel.CurrentLookDirection = direction;
    }

    #region Player Animation Change

    private float GetAnimationClipLength(PlayerAnimationStates animation)
    {
        Animator anim = playerView.PlayerAnimator;
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
        if (playerModel.CurrentAnimation == animation || playerModel.CurrentAnimation == PlayerAnimationStates.Dead)
            return;
        playerView.PlayerAnimator.Play(animation.ToString());
        playerModel.CurrentAnimation = animation;
    }

    private void PlayPlayerFightAnimation()
    {
        switch (playerModel.CurrentLookDirection)
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
        switch (playerModel.CurrentLookDirection)
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
        switch (playerModel.CurrentLookDirection)
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

    private async void ResetPlayerAnimation()
    {
        await Task.Delay((int)(1000f * GetAnimationClipLength(playerModel.CurrentAnimation)));
        if (playerView.CurrentState == playerView.PlayerIdelState)
            PlayPlayerIdelAnimation();
        else
            PlayPlayerRunningAnimation();
    }
    #endregion
    public void PlayerAttack()
    {
        PlayPlayerFightAnimation();
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerView.AttackPoint.position, playerModel.AttackRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponent<IDamageable>()?.TakeDamage();
        }
        // wait for animation to done playing and put debug.log("Done playing")
        ResetPlayerAnimation();
    }
    public void ReduceHealth(int attackPower)
    {
        ChangePlayerAnimation(PlayerAnimationStates.Took_Damage);
        playerModel.CurrentHealth -= attackPower;
        if (playerModel.CurrentHealth <= 0)
        {
            PlayerDead();
            return;
        }
        ResetPlayerAnimation();

    }
    

    

    private void PlayerDead()
    {
        ChangePlayerAnimation(PlayerAnimationStates.Dead);
        playerView.enabled = false;
        GameManager.Instance.PlayedDied();
    }
}
